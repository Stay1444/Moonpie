using Moonpie.Plugins;
using Moonpie.Plugins.Attributes;
using Moonpie.Protocol.Protocol;

namespace BasicPlugin;

public class BasicCommands : BaseCommandModule
{
    [Command("hello")]
    public async Task HelloCommand(CommandContext ctx)
    {
        await ctx.Player.SendMessageAsync("Hello World!");
    }
    
    [Command("test")]
    public async Task TestCommand(CommandContext ctx, string host, int port)
    {
        await ctx.Player.SendMessageAsync("Connecting you to " + host + ":" + port + "...");
        await Task.Delay(1000);
        await ctx.Player.Connect(host, (uint)port);
        await ctx.Player.SendMessageAsync("Connected!");
    }

    [Command("transport")]
    public async Task TransportCommand(CommandContext ctx)
    {
        await ctx.Player.SendMessageLinesAsync(
            ChatComponent.Parse($"{ctx.Player.Username} §a<->§r §9Moonpie§r §a<->§r Server"),
            ChatComponent.Parse($"{ctx.Player.Username} §a<->§r §9Moonpie§r [§cNot Encrypted§r] [{(ctx.Player.Transport.PlayerConnection.CompressionEnabled ? $"§bCompressed ({ctx.Player.Transport.PlayerConnection.CompressionThreshold})§r" : "§cNot Compressed§r")}]"),
            ChatComponent.Parse($"Server §a<->§r §9Moonpie§r [§cNot Encrypted§r] [{(ctx.Player.Transport.ServerConnection!.CompressionEnabled ? $"§bCompressed ({ctx.Player.Transport.ServerConnection.CompressionThreshold})§r" : "§cNot Compressed§r")}]")
            );
    }

    [Command("text")]
    public async Task TextCommand(CommandContext ctx, int length)
    {
        string text = "";
        for (int i = 0; i < length; i++)
        {
            text += "a";
        }
        
        await ctx.Player.SendMessageAsync(text);
    }
}