using System.Text.Json.Serialization;

namespace Moonpie.Protocol.Protocol.Mapping;

public class RawMapVersionModel
{
    [JsonPropertyName("c2s")]
    public string[]? C2S { get; set; }
    [JsonPropertyName("s2c")]
    public string[]? S2C { get; set; }
}