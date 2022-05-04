using System.Text.Json.Serialization;

namespace Moonpie.Mojang.Models;

public record FullProfileModel(string Id, string Name)
{
    public record ProfileProperties(string Name, string Value, string? Signature);
    
    [JsonIgnore]
    public Guid IdGuid => Guid.ParseExact(Id, "N");
    public List<ProfileProperties> Properties { get; init; } = new List<ProfileProperties>();
}