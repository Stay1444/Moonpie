using Moonpie.Protocol.Network;
using Moonpie.Protocol.Protocol;

namespace Moonpie.Protocol.Packets.c2s.Status;

[PacketType(PacketTypes.C2S.STATUS_REQUEST)]
public class StatusRequestC2SP : IC2SPacket
{
    public void Read(InByteBuffer buffer)
    {
        
    }

    public void Write(OutByteBuffer buffer)
    {
        
    }
}