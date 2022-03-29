#region Copyright
// Moonpie
// 
// Copyright (c) 2022 Stay
// 
// MIT License
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
#endregion

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