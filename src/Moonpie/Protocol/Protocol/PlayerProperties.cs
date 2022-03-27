using System.Text.Json;

namespace Moonpie.Protocol.Protocol;

public class PlayerProperties
{
    private const string URL = "https://sessionserver.mojang.com/session/minecraft/profile/{uuid}?unsigned=false";
    
    public PlayerTextures? Textures { get; set; }

    public static async Task<PlayerProperties> Fetch(Guid uuid)
    {
        var url = URL.Replace("{uuid}", uuid.ToString());
        var httpClient = new HttpClient();
        var response = await httpClient.GetAsync(url);
        
        PlayerTextures? textures = null;
        
        dynamic json = JsonSerializer.Deserialize<dynamic>(await response.Content.ReadAsStringAsync())!;
        
        List<object> properties = json.properties;
        
        foreach (var property in properties)
        {
            if (property is Dictionary<object, object> dictionary)
            {
                var name = (string?)dictionary.FirstOrDefault(x => (string) x.Key == "name").Value;
                if (name == "textures")
                {
                    textures = PlayerTextures.From((string)dictionary["value"], (string)dictionary["signature"]);
                }
            }
        }
        
        return new PlayerProperties()
        {
            Textures = textures
        };
    }
    
}