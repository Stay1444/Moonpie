using Moonpie.Protocol.Network;
using Moonpie.Protocol.Protocol;

namespace Moonpie.Protocol.Packets.c2s.Status;

[PacketType(PacketTypes.C2S.STATUS_PING)]
public class StatusPingC2SP : IC2SPacket
{
    public long PingId { get; set; }
    
    public void Read(InByteBuffer buffer)
    {
        PingId = buffer.ReadLong();
    }

    public void Write(OutByteBuffer buffer)
    {
        buffer.WriteLong(PingId);
    }
}