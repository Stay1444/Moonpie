using Moonpie.Protocol.Network;
using Moonpie.Protocol.Protocol;

namespace Moonpie.Protocol.Packets.s2c.Status;

[PacketType(PacketTypes.S2C.STATUS_PONG)]
public class StatusPongS2CP : IS2CPacket
{
    public long PingId { get; set; }

    public void Log()
    {
        Console.WriteLine($"StatusPongS2CP: PingId: {PingId}");
    }

    public void Read(InByteBuffer buffer)
    {
        PingId = buffer.ReadLong();
    }

    public void Write(OutByteBuffer buffer)
    {
        buffer.WriteLong(PingId);
    }
}