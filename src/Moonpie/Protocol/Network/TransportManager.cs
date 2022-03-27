using System.Net.Sockets;
using Moonpie.Entities;
using Moonpie.Protocol.Packets.c2s.Handshaking;
using Moonpie.Protocol.Packets.c2s.Login;
using Moonpie.Protocol.Protocol;
using Serilog;

namespace Moonpie.Protocol.Network;

public class TransportManager
{
    public event EventHandler? PlayerDisconnected;
    private Player _player;
    public PlayerConnection PlayerConnection { get; private set; }
    public ServerConnection? ServerConnection { get; private set; }
    private PacketHandler _handler;
    public ProtocolVersion Version => PlayerConnection.Version;
    public TransportManager(Player player, PlayerConnection playerConnection)
    {
        _player = player;
        var proxy = player.Proxy;
        PlayerConnection = playerConnection;
        _handler = new PacketHandler(proxy, _player, this);
    }

    internal async Task Connect(string host, uint port)
    {
        TcpClient client = new TcpClient();
        await client.ConnectAsync(host, (int)port);
        ServerConnection = new ServerConnection(client, Version);
        ServerConnection.State = ProtocolState.Login;

        await ServerConnection.WritePacketAsync(new HandshakeC2SP()
        {
            ProtocolVersion = Version,
            ServerAddress = host,
            ServerPort = (ushort)port,
            NextState = ProtocolState.Login,
        });

        await ServerConnection.WritePacketAsync(new LoginStartC2SP()
        {
            Username = _player.Username
        });

        _ = ServerRead();
        _ = PlayerRead();

    }

    private async Task PlayerRead()
    {
        try
        {
            while (PlayerConnection.IsConnected && ServerConnection is not null && ServerConnection.IsConnected)
            {
                var packet = await PlayerConnection.ReadPacketAsync();
                if (packet is null)
                {
                    break;
                }
                if (!await _handler.Handle(packet!))
                {
                    await ServerConnection.WritePacketAsync(packet!);
                }
            }

            if (ServerConnection is not null)
            {
                PlayerDisconnected?.Invoke(_player, EventArgs.Empty);
                ServerConnection?.DisconnectAsync();
                ServerConnection?.Dispose();
                ServerConnection = null;
            }
        }
        catch (Exception e)
        {
            Log.Error(e, "Player read error");
        }
    }

    private async Task ServerRead()
    {
        while (PlayerConnection.IsConnected && ServerConnection is not null && ServerConnection.IsConnected)
        {
            var packet = await ServerConnection.ReadPacketAsync();
            if (packet is null)
            {
                break;
            }

            if (!await _handler.Handle(packet!))
            {
                await PlayerConnection.WritePacketAsync(packet!);
            }
        }

        if (ServerConnection is not null)
        {
            PlayerDisconnected?.Invoke(_player, EventArgs.Empty);
            await PlayerConnection.DisconnectAsync();
            PlayerConnection.Dispose();
            ServerConnection = null;
        }
    }
}