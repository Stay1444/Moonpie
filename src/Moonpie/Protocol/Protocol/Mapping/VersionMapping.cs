
namespace Moonpie.Protocol.Protocol.Mapping;

public class VersionMapping
{
    public ProtocolVersion? Version { get; set; }
    public ProtocolVersion? MapVersion { get; set; }
    public List<PacketTypes.S2C>? S2C { get; set; }
    public List<PacketTypes.C2S>? C2S { get; set; }
    
    public VersionMapping()
    {
        S2C = new List<PacketTypes.S2C>();
        C2S = new List<PacketTypes.C2S>();
    }
}