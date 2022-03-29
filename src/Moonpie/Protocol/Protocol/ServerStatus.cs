using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Moonpie.Protocol.Protocol;

public class ServerStatus
{
    public class ServerStatusPlayers
    {
        public class ServerStatusPlayersSample
        {
            public string Name { get; set; } = "";
            public string Id { get; set; } = "";
        }
        
        [JsonPropertyName("max")]
        public int Max { get; set; } = 0;
        [JsonPropertyName("online")]
        public int Online { get; set; } = 0;

        [JsonPropertyName("sample"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<ServerStatusPlayersSample>? Sample { get; set; } = null;
    }

    public class ServerStatusVersion
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = "";
        [JsonPropertyName("protocol")]
        public int Protocol { get; set; } = 0;
    }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Favicon { get; set; }
    
    [JsonPropertyName("description")]
    public ChatComponent Description { get; set; } = ChatComponent.Empty;
    
    [JsonPropertyName("players")]
    public ServerStatusPlayers Players { get; set; } = new ServerStatusPlayers();
    
    [JsonPropertyName("version")]
    public ServerStatusVersion Version { get; set; } = new ServerStatusVersion();
    
}