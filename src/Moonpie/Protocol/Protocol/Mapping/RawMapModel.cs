using System.Text.Json.Serialization;

namespace Moonpie.Protocol.Protocol.Mapping;

public class RawMapModel
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    
    [JsonPropertyName("mapping")]
    public object? Mapping { get; set; }
    
    [JsonPropertyName("protocol_id")]
    public int? ProtocolId { get; set; }
}