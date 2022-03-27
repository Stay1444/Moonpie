using System.Text.Json.Serialization;

namespace Moonpie.Protocol.Protocol;

public class ChatComponent
{
    [JsonPropertyName("text")]
    public string Text { get; set; }
    
    [JsonPropertyName("extra"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<ChatComponent>? Extra { get; set; } 

    [JsonPropertyName("color"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull), JsonConverter(typeof(ChatColorJsonConverter))]
    public ChatColor? Color { get; set; }
    public ChatComponent Add(ChatComponent component)
    {
        if (Extra == null)
        {
            Extra = new List<ChatComponent>();
        }
        Extra.Add(component);
        return this;
    }
    
    public static ChatComponent Empty
    {
        get
        {
            return new ChatComponent("");
        }
    }
    
    public ChatComponent(string text)
    {
        this.Text = text;
    }
    
    public static implicit operator ChatComponent(string text)
    {
        return new ChatComponent(text);
    }

    public static ChatComponent Parse(string text, char colorChar = '§')
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
}