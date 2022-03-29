using System.Net.Sockets;
using System.Threading.Tasks;
using Moonpie.Protocol.Packets.c2s;

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