#region Copyright
// Moonpie
// 
// Copyright (c) 2022 Stay
// 
// MIT License
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
#endregion

using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Moonpie.Entities;
using Moonpie.Protocol.Packets;
using Moonpie.Protocol.Packets.c2s.Handshaking;
using Moonpie.Protocol.Packets.c2s.Login;
using Moonpie.Protocol.Packets.s2c.Login;
using Moonpie.Protocol.Protocol;
using Moonpie.Utils.Protocol;
using Serilog;

namespace Moonpie.Protocol.Network;

public class TransportManager
{
    public class Transport
    {
        private readonly Connection _connection;
        public Connection Connection => _connection;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private Task? _task;
        internal event Action<Transport>? OnDisconnected;
        private readonly Func<IPacket, Task> _onPacketReceived;
        internal Transport(Connection connection, Func<IPacket, Task> callback)
        {
            _connection = connection;
            _cancellationTokenSource = new CancellationTokenSource();
            _onPacketReceived = callback;
            _connection.Disconnected += (t, a) =>
            {
                _cancellationTokenSource.Cancel();
                OnDisconnected?.Invoke(this);
                _connection.Dispose();
            };
        }

        private async Task Read()
        {
            try
            {
                while (!_cancellationTokenSource.IsCancellationRequested && _connection.IsConnected)
                {
                    IPacket? packet;
                    if (_connection is PlayerConnection playerConnection)
                    {
                        packet = await playerConnection.ReadPacketAsync();
                    }
                    else if (_connection is ServerConnection serverConnection)
                    {
                        packet = await serverConnection.ReadPacketAsync();
                    }
                    else
                    {
                        throw new Exception("Unknown connection type");
                    }

                    if (packet is null)
                    {
                        _connection.Dispose();
                        _cancellationTokenSource.Cancel();
                        OnDisconnected?.Invoke(this);
                        return;
                    }

                    await _onPacketReceived(packet);
                }

                if (_cancellationTokenSource.IsCancellationRequested)
                {
                    _connection.Dispose();
                    return;
                }

                if (!_connection.IsConnected)
                {
                    _connection.Dispose();
                    _cancellationTokenSource.Cancel();
                    OnDisconnected?.Invoke(this);
                    return;
                }
            }
            catch (OperationCanceledException)
            {
                _connection.Dispose();
            }
            catch (Exception e)
            {
                Log.Error(e, "Player read error");
            }
        }

        internal void Begin()
        {
            _task = Read();
        }

        internal async Task Close()
        {
            _cancellationTokenSource.Cancel();
            if (_task is not null)
            {
                await _task;
            }
        }
        
    }
    
    public event EventHandler? PlayerDisconnected;
    private Player _player;
    private PacketHandler _handler;
    private JavaUUID _uuid;
    public ProtocolVersion Version => _playerTransport.Connection.Version;
    
    private Transport _playerTransport;
    private Transport? _serverTransport;
    public Transport PlayerTransport => _playerTransport;
    public Transport? ServerTransport => _serverTransport;
    public TransportManager(Player player, PlayerConnection playerConnection)
    {
        _player = player;
        var proxy = player.Proxy;
        _handler = new PacketHandler(proxy, _player, this);
        _playerTransport = new Transport(playerConnection, HandlePlayerRead);
        _playerTransport.Begin();
        _playerTransport.OnDisconnected += HandlePlayerDisconnected;
    }

    private void HandlePlayerDisconnected(Transport transport)
    {
        PlayerDisconnected?.Invoke(_player, EventArgs.Empty);
        _serverTransport?.Close();
        _playerTransport.OnDisconnected -= HandlePlayerDisconnected;
    }

    private async Task HandlePlayerRead(IPacket packet)
    {
        if (!await _handler.Handle(packet))
        {
            await _serverTransport!.Connection.WritePacketAsync(packet);
        }
    }

    private async Task HandleServerRead(IPacket packet)
    {
        if (!await _handler.Handle(packet))
        {
            await _playerTransport.Connection.WritePacketAsync(packet);
        }
    }
    
    internal async Task Connect(string host, uint port)
    {
        try
        {
            if (_serverTransport is not null)
            {
                await TransportConnection(host, port);
                return;
            }

            var server = await CreateServerConnection(host, port);
        
            if (server is null) return;
            
            await _playerTransport.Connection.WritePacketAsync(new CompressionSetS2CP()
            {
                Threshold = _player.Proxy.Configuration.CompressionThreshold
            });
            _playerTransport.Connection.CompressionThreshold = _player.Proxy.Configuration.CompressionThreshold;
            
            await server.WritePacketAsync(new HandshakeC2SP()
            {
                ProtocolVersion = Version,
                ServerAddress = server.RemoteEndPoint.GetHost(),
                ServerPort = (ushort) server.RemoteEndPoint.GetPort(),
                NextState = ProtocolState.Login,
            });
            server.State = ProtocolState.Login;

            await server.WritePacketAsync(new LoginStartC2SP()
            {
                Username = _player.Username
            });

            {
                var packet = await server.ReadPacketAsync();
                if (packet is CompressionSetS2CP compressionSetS2CP)
                {
                    server.CompressionThreshold = compressionSetS2CP.Threshold;
                    packet = await server.ReadPacketAsync();
                }

                if (packet is not LoginSuccessS2CP)
                {
                    server.Dispose();
                    throw new Exception("Login failed");
                }
                
                var loginSuccessS2CP = (LoginSuccessS2CP) packet;
                this._uuid = loginSuccessS2CP.Uuid;
                _player.Username = loginSuccessS2CP.Name ?? _player.Username;
                Console.WriteLine(_uuid);

                await _playerTransport.Connection.WritePacketAsync(packet);
                
                server.State = ProtocolState.Play;
                _playerTransport.Connection.State = ProtocolState.Play;

                _serverTransport = new Transport(server, HandleServerRead);
                _serverTransport.Begin();
            }
        }
        catch (Exception e)
        {
            Log.Error(e, "Failed to connect to server");
            throw;
        }
    }

    private async Task<ServerConnection?> CreateServerConnection(string host, uint port)
    {
        TcpClient client = new TcpClient();
        await client.ConnectAsync(host, (int)port);
        
        var server = new ServerConnection(client, Version);
        return server;
    }
    private async Task TransportConnection(string host, uint port)
    {
        TcpClient client = new TcpClient();
        await client.ConnectAsync(host, (int)port);

        var server = await CreateServerConnection(host, port);

        if (server is null)
        {
            throw new Exception("Failed to connect to server");
        }
        
        await server.WritePacketAsync(new HandshakeC2SP()
        {
            ProtocolVersion = Version,
            ServerAddress = server.RemoteEndPoint.GetHost(),
            ServerPort = (ushort) server.RemoteEndPoint.GetPort(),
            NextState = ProtocolState.Login,
        });
        server.State = ProtocolState.Login;
        await server.WritePacketAsync(new LoginStartC2SP()
        {
            Username = _player.Username
        });
        var packet = await server.ReadPacketAsync();
        if (packet is CompressionSetS2CP compressionSetS2CP)
        {
            server.CompressionThreshold = compressionSetS2CP.Threshold;
            packet = await server.ReadPacketAsync();
        }
        
        if (packet is not LoginSuccessS2CP)
        {
            server.Dispose();
            throw new Exception("Login failed");
        }
        
        var loginSuccessS2CP = (LoginSuccessS2CP) packet;
        this._uuid = loginSuccessS2CP.Uuid;
        Console.WriteLine(_uuid);
        _player.Username = loginSuccessS2CP.Name ?? _player.Username;
        
        if (_serverTransport is not null)
        {
            await _serverTransport.Close();
        }

        _serverTransport = new Transport(server, HandleServerRead);
        server.State = ProtocolState.Play;
        _serverTransport.Begin();
    }

}