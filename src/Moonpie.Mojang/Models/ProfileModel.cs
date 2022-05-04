using System.Text.Json.Serialization;

namespace Moonpie.Mojang.Models;

public record ProfileModel(string Id, string Name, bool Legacy, bool Demo)
{
    [JsonIgnore]
    public Guid IdGuid => Guid.ParseExact(Id, "N");
}