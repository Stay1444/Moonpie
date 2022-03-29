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
    
    [Command("connect")]
    public async Task TestCommand(CommandContext ctx, string host, int port)
    {
        await ctx.Player.SendMessageAsync("Connecting you to " + host + ":" + port + "...");
        await ctx.Player.Connect(host, (uint)port);
        await ctx.Player.SendMessageAsync("Connected!");
    }

    [Command("transport")]
    public async Task TransportCommand(CommandContext ctx)
    {
        await ctx.Player.SendMessageLinesAsync(
            ChatComponent.Parse($"{ctx.Player.Username} §a<->§r §9Moonpie§r §a<->§r Server"),
            ChatComponent.Parse($"{ctx.Player.Username} §a<->§r §9Moonpie§r [§cNot Encrypted§r] [{(ctx.Player.Transport.PlayerTransport.Connection.CompressionEnabled ? $"§bCompressed ({ctx.Player.Transport.PlayerTransport.Connection.CompressionThreshold})§r" : "§cNot Compressed§r")}]"),
            ChatComponent.Parse($"Server §a<->§r §9Moonpie§r [§cNot Encrypted§r] [{(ctx.Player.Transport.PlayerTransport.Connection!.CompressionEnabled ? $"§bCompressed ({ctx.Player.Transport.ServerTransport!.Connection.CompressionThreshold})§r" : "§cNot Compressed§r")}]")
            );
    }
}