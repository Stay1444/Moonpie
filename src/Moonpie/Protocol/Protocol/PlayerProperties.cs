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
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

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