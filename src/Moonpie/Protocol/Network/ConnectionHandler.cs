#region License
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
using System.Threading.Tasks;
using Moonpie.Entities;
using Moonpie.Entities.Models;
using Moonpie.Entities.Models.Events;
using Moonpie.Protocol.Packets.c2s.Handshaking;
using Moonpie.Protocol.Packets.c2s.Login;
using Moonpie.Protocol.Packets.c2s.Status;
using Moonpie.Protocol.Packets.s2c.Login;
using Moonpie.Protocol.Packets.s2c.Status;
using Moonpie.Protocol.Protocol;
using Serilog;

namespace Moonpie.Protocol.Network;

public class ConnectionHandler
{
    private MoonpieConfiguration _config;
    private Moonpie _proxy;
    public ConnectionHandler(MoonpieConfiguration config, Moonpie proxy)
    {
        _config = config;
        _proxy = proxy;
    }

    public async Task<Player?> HandleConnectionAsync(TcpClient client)
    {
        try
        {
            
            var playerConnection = new PlayerConnection(client);
            
            var packet = await playerConnection.ReadPacketAsync();

            if (packet is not null && packet is HandshakeC2SP handshakeC2SP)
            {
                playerConnection.State = handshakeC2SP.NextState;
                playerConnection.Version = handshakeC2SP.ProtocolVersion;
            }else
            {
                Log.Error("Invalid packet received");
                await playerConnection.DisconnectAsync();
                playerConnection.Dispose();
                return null;
            }

            if (playerConnection.State == ProtocolState.Status)
            {
                await HandleStatusRequest(playerConnection);
                return null;
            }

            if (playerConnection.State == ProtocolState.Login)
            {
                return await HandleLoginRequest(playerConnection);
            }
            
        }catch(Exception e)
        {
            Log.Error(e, "Error handling connection");
            client.Close();
        }
        
        return null;
    }

    private async Task HandleStatusRequest(PlayerConnection connection)
    {
        var result = await _proxy.Plugins.TriggerEventAsync(new PlayerPingEventArgs(_proxy, connection.RemoteEndPoint, connection.Version));
        if (result is null)
        {
            await connection.DisconnectAsync();
            connection.Dispose();
            return;
        }

        if (result.Canceled)
        {
            await connection.DisconnectAsync();
            connection.Dispose();
            return;
        }

        if (result.Response is null)
        {
            result.Response = new ServerStatusResponseBuilder();
            result.Response.WithDescription("Moonpie");
            result.Response.WithVersion(connection.Version);
            result.Response.WithPlayers(_proxy.Players.Count, (int)_config.Net.MaxConnections);
        }

        if (await connection.ReadPacketAsync() is StatusRequestC2SP)
        {
            
            ServerStatusResponseS2CP response = new ServerStatusResponseS2CP();
            response.Status = new ServerStatus()
            {
                Version = new ServerStatus.ServerStatusVersion()
                {
                    Name = result.Response.Version.Name,
                    Protocol = result.Response.Version.Value
                },
                Description = result.Response.Description,
                Players = new ServerStatus.ServerStatusPlayers()
                {
                    Online = result.Response.UsedSlots,
                    Max = result.Response.MaxSlots
                }
            };

            await connection.WritePacketAsync(response);

        }else
        {
            Log.Error("Invalid packet received");
            await connection.DisconnectAsync();
            connection.Dispose();
            return;
        }

        if (await connection.ReadPacketAsync() is StatusPingC2SP ping)
        {
            await connection.WritePacketAsync(new StatusPongS2CP()
            {
                PingId = ping.PingId
            });
            Log.Information("Ping {0}", connection.RemoteEndPoint);
        }else
        {
            Log.Error("Invalid packet received");
            await connection.DisconnectAsync();
            connection.Dispose();
        }
    }

    private async Task<Player?> HandleLoginRequest(PlayerConnection connection)
    {
        var loginPacket = await connection.ReadPacketAsync();
        if (loginPacket is null) return null;
        string? username;
        if (loginPacket is LoginStartC2SP loginStartC2SP)
        {
            username = loginStartC2SP.Username;
        }
        else
        {
            connection.Dispose();
            return null;
        }
        
        if (username is null)
        {
            connection.Dispose();
            return null;
        }
        
        if (_proxy.Players.Any(x => x.Username == username))
        {
            await connection.WritePacketAsync(new LoginKickS2CP()
            {
                Reason = "Username already in use"
            });
            await connection.DisconnectAsync();
            connection.Dispose();
            return null;
        }
        
        var player = new Player(_proxy, connection, username);
        try
        {
            string address = _config.DefaultServer.Host.Split(":")[0];
            int port = int.Parse(_config.DefaultServer.Host.Split(":")[1]);
            await player.Connect(address, port);
            return player;
        }
        catch (Exception e)
        {
            Log.Error(e, "Error connecting to fallback server");
            await connection.DisconnectAsync();
            connection.Dispose();
            return null;
        }
    }
}