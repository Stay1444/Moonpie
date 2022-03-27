using Moonpie.Protocol.Network;

namespace Moonpie.Protocol.Packets;

public interface IPacketProvider
{
    public IPacket? GetPacket(InByteBuffer buffer);
}