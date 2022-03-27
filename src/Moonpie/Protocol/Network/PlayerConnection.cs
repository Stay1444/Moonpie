using System.Net;
using System.Net.Sockets;
using Moonpie.Protocol.Packets;
using Moonpie.Protocol.Packets.c2s;
using Moonpie.Protocol.Packets.s2c;
using Moonpie.Protocol.Packets.s2c.Play;
using Moonpie.Protocol.Protocol;
using Moonpie.Utils.Protocol;
using Serilog;

namespace Moonpie.Protocol.Network;

public class PlayerConnection : Connection
{
    public PlayerConnection(TcpClient client) : base(client)
    {
        
    }

    public Task<IC2SPacket?> ReadPacketAsync()
    {
        return ReadC2SPacketAsync();
    }
}