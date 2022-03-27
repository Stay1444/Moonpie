using System.Drawing;
using System.Text.Json;
using System.Text.Json.Serialization;
using Ardalis.SmartEnum;

namespace Moonpie.Protocol.Protocol;

public class ChatColor : SmartEnum<ChatColor, string>
{
    public static readonly ChatColor Black = new ChatColor("Black", "black", '0', Color.Black);
    public static readonly ChatColor DarkBlue = new ChatColor("DarkBlue", "dark_blue", '1', Color.DarkBlue);
    public static readonly ChatColor DarkGreen = new ChatColor("DarkGreen", "dark_green",'2', Color.DarkGreen);
    public static readonly ChatColor DarkCyan = new ChatColor("DarkAqua", "dark_aqua",'3', Color.DarkCyan);
    public static readonly ChatColor DarkRed = new ChatColor("DarkRed", "dark_red",'4', Color.DarkRed);
    public static readonly ChatColor DarkPurple = new ChatColor("DarkPurple", "dark_purple",'5', Color.Purple);
    public static readonly ChatColor Gold = new ChatColor("Gold", "gold", '6',Color.Gold);
    public static readonly ChatColor Gray = new ChatColor("Gray", "gray", '7',Color.Gray);
    public static readonly ChatColor DarkGray = new ChatColor("DarkGray", "dark_gray",'8', Color.DarkGray);
    public static readonly ChatColor Blue = new ChatColor("Blue", "blue",'9', Color.Blue);
    public static readonly ChatColor BrightGreen = new ChatColor("Green","green",'a', Color.LightGreen);
    public static readonly ChatColor Aqua = new ChatColor("Aqua", "aqua",'b', Color.Aqua);
    public static readonly ChatColor Red = new ChatColor("Red", "red",'c', Color.Red);
    public static readonly ChatColor LightPurple = new ChatColor("Pink", "light_purple",'d', Color.Plum);
    public static readonly ChatColor Yellow = new ChatColor("Yellow", "yellow",'e', Color.Yellow);
    public static readonly ChatColor White = new ChatColor("White", "white",'f', Color.White);
    public static readonly ChatColor Reset = new ChatColor("Reset", "reset",'r', Color.White);
    internal string Data { get; }
    public Color Color { get; }
    public string Name { get; }
    public char Code { get; }
    private ChatColor(string name, string data, char code, Color color) : base(name,data)
    {
        this.Name = name;
        this.Data = data;
        this.Color = color;
        this.Code = code;
    }
}

public class ChatColorJsonConverter : JsonConverter<ChatColor>
{
    public override ChatColor? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return ChatColor.FromValue(reader.GetString() ?? "white");
    }

    public override void Write(Utf8JsonWriter writer, ChatColor value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Data);
    }
}