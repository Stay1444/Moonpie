using System.Dynamic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Moonpie.Utils.Mojang;

namespace Moonpie.Protocol.Protocol;

public class PlayerTextures
{
    [JsonPropertyName("name"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Name { get; set; }

    [JsonPropertyName("uuid"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Guid? Uuid { get; set; }
    
    [JsonPropertyName("date"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DateTime? Date { get; set; }
    
    [JsonPropertyName("skin"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public SkinPlayerTexture? Skin { get; set; }
    
    [JsonPropertyName("cape"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public PlayerTexture? Cape { get; set; }
    
    [JsonPropertyName("elytra"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public PlayerTexture? Elytra { get; set; }

    public static PlayerTextures From(string encoded, string signature)
    {
        byte[] encodedBytes = Convert.FromBase64String(encoded);
        string encodedFromBase64 = Encoding.UTF8.GetString(encodedBytes);
        dynamic json = JsonSerializer.Deserialize<ExpandoObject>(encodedFromBase64)!;
        return new PlayerTextures
        {
            Name = json.profileName,
            Uuid = json.profileId,
            Date = json.timestamp,
            Skin = json.SKIN,
            Cape = json.CAPE,
            Elytra = json.ELYTRA
        };
    }
}