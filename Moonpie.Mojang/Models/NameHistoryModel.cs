using System.Text.Json.Serialization;

namespace Moonpie.Mojang.Models;

public record NameHistoryModel(string Name, long ChangedToAt)
{
    [JsonIgnore]
    public DateTime ChangedAt => DateTimeOffset.FromUnixTimeMilliseconds(ChangedToAt).DateTime;
}