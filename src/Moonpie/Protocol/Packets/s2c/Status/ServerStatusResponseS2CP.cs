using Moonpie.Protocol.Network;
using Moonpie.Protocol.Protocol;
using Moonpie.Utils.Protocol;

namespace Moonpie.Protocol.Packets.s2c.Status;

[PacketType(PacketTypes.S2C.STATUS_RESPONSE)]
public class ServerStatusResponseS2CP : IS2CPacket
{
    
    public ServerStatus? Status { get; set; }
    
    public void Log()
    {
        Console.WriteLine("ServerStatusResponseS2CP");
    }

    public void Read(InByteBuffer buffer)
    {
        Status = buffer.ReadJson<ServerStatus>();
    }

    public void Write(OutByteBuffer buffer)
    {
        buffer.WriteJson(Status ?? new ServerStatus());
    }
}