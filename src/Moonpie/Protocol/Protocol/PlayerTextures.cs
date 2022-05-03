#region License
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

using System.Dynamic;
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