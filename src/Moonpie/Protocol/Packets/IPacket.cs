using Moonpie.Protocol.Network;

namespace Moonpie.Protocol.Packets;

public interface IPacket
{
    public void Read(InByteBuffer buffer);
    public void Write(OutByteBuffer buffer);

    public Task Handle(PacketHandleContext handler)
    {
        return Task.FromResult(false);
    }
}