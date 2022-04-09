﻿#region License
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

using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Moonpie.Protocol.Protocol;

public class ChatComponent
{
    [JsonPropertyName("text"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Text { get; set; }
    
    [JsonPropertyName("extra"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<ChatComponent>? Extra { get; set; } 

    [JsonPropertyName("color"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull), JsonConverter(typeof(ChatColorJsonConverter))]
    public ChatColor? Color { get; set; }
    public ChatComponent Add(ChatComponent component)
    {
        Extra ??= new List<ChatComponent>();
        Extra.Add(component);
        return this;
    }
    
    public static ChatComponent Empty => new ChatComponent("");

    public ChatComponent(string text)
    {
        this.Text = text;
    }
    
    public static implicit operator ChatComponent(string text)
    {
        return new ChatComponent(text);
    }

    public static ChatComponent Parse(string text, char colorChar = ChatColor.PrefixChar)
    {
        var result = new ChatComponent("");
        var builder = new List<char>();
        var lastColor = ChatColor.White;
        for(int c = 0; c < text.Length; c++)
        {
            if (text[c] != colorChar)
            {
                builder.Add(text[c]);
                continue;
            }

            if (c + 1 >= text.Length) continue;

            if (text[c + 1] == colorChar)
            {
                builder.Add(colorChar);
                c++;
                continue;
            }
            
            char next = text[c + 1];
            if (ChatColor.List.Any(x => x.Code == next))
            {
                if (string.IsNullOrEmpty(result.Text))
                {
                    result.Text = new string(builder.ToArray());
                    result.Color = lastColor;
                }
                else
                {
                    result.Add(new ChatComponent(new string(builder.ToArray()))
                    {
                        Color = lastColor
                    });
                }
                lastColor = ChatColor.List.First(x => x.Code == next);
                builder.Clear();
                c++;
            }
        }
        
        if (builder.Count > 0)
        {
            if (string.IsNullOrEmpty(result.Text))
            {
                result.Text = new string(builder.ToArray());
                result.Color = lastColor;
            }
            else
            {
                result.Add(new ChatComponent(new string(builder.ToArray()))
                {
                    Color = lastColor
                });
            }
        }
        
        return result;
    }
    
    public override bool Equals(object? obj)
    {
        if (obj is ChatComponent component)
        {
            if (component.Text != null || this.Text != null)
            {
                if (component.Text != this.Text)
                {
                    return false;
                }
            }
            
            if (component.Color != null || this.Color != null)
            {
                if (component.Color != this.Color)
                {
                    return false;
                }
            }
            
            if (component.Extra != null || this.Extra != null)
            {
                if (component.Extra != null && this.Extra != null)
                {
                    if (!component.Extra.SequenceEqual(this.Extra))
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            
            return true;
            
        }
        return false;
    }
    
    public bool IsEmpty()
    {
        if (string.IsNullOrEmpty(Text)) return true;

        if (Extra is not null)
        {
            foreach (var chatComponent in Extra)
            {
                if (!chatComponent.IsEmpty()) return false;
            }
            
            return true;
        }else
        {
            return false;
        }
    }

    private static ChatComponent FromQuotedString(string str)
    {
        var unquoted = str.Substring(1, str.Length - 2);
        
        return Parse(unquoted);
    }
 
    public string ToQuotedString()
    {
        var builder = new StringBuilder();
        
        builder.Append('"');
        if (this.Color is not null)
        {
            builder.Append(ChatColor.PrefixChar);
            builder.Append(this.Color.Code);
        }
        
        builder.Append(this.Text);
        if (this.Extra is not null)
        {
            foreach (var chatComponent in this.Extra)
            {
                if (chatComponent.IsEmpty()) continue;
                if (chatComponent.Color is not null)
                {
                    builder.Append(ChatColor.PrefixChar);
                    builder.Append(chatComponent.Color.Code);
                }
                
                builder.Append(chatComponent.Text);
            }
        }
        
        builder.Append('"');
        
        return builder.ToString();
    }

    public enum SerializationMode
    {
        Normal,
        Compact,
        Quoted
    }

    private SerializationMode _serializationMode = SerializationMode.Normal;
    
    public static ChatComponent FromJson(string json)
    {
        if (json.StartsWith('"'))
        {
            var cc = FromQuotedString(json);
            
            cc._serializationMode = SerializationMode.Quoted;
            
            return cc;
        }

        try
        {
            return JsonSerializer.Deserialize<ChatComponent>(json)!;
        }
        catch (JsonException)
        {
            var compact = JsonSerializer.Deserialize<ChatComponentCompact>(json);
            var chatComponent = ChatComponent.Empty;
            chatComponent.Text = compact?.Text;
            if (compact?.Extra is not null)
            {
                foreach (var extra in compact.Extra)
                {
                    chatComponent.Add(new ChatComponent(extra));
                }
            }
            
            chatComponent._serializationMode = SerializationMode.Compact;
            
            return chatComponent;
        }
    }

    public class ChatComponentCompact
    {
        [JsonPropertyName("text"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Text { get; set; }
        
        [JsonPropertyName("extra"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? Extra { get; set; }
    }
}
