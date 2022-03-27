using System.Net.Sockets;
using Moonpie.Protocol.Packets.s2c;
using Moonpie.Protocol.Protocol;

namespace Moonpie.Protocol.Network;

public class ServerConnection : Connection
{
    public ServerConnection(TcpClient client) : base(client)
    {
    }

    public ServerConnection(TcpClient client, ProtocolVersion version) : base(client)
    {
        this.Version = version;
    }

    public Task<IS2CPacket?> ReadPacketAsync()
    {
        return ReadS2CPacketAsync();
    }
}