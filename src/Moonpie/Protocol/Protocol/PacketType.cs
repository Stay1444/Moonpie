namespace Moonpie.Protocol.Protocol;

public class PacketType : Attribute
{
    public PacketTypes.C2S? C2S { get; }
    public PacketTypes.S2C? S2C { get; }
    public PacketType(PacketTypes.C2S p)
    {
        C2S = p;    
    }
    
    public PacketType(PacketTypes.S2C p)
    {
        S2C = p;
    }
}