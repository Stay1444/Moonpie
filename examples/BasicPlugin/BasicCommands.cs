using Moonpie.Plugins;
using Moonpie.Plugins.Attributes;
using Moonpie.Protocol.Packets.s2c.Play;
using Moonpie.Protocol.Protocol;
using Serilog;

namespace BasicPlugin;

public class BasicCommands : BaseCommandModule
{
    [Command("hello")]
    public async Task HelloCommand(CommandContext ctx)
    {
        await ctx.Player.SendMessageAsync("Hello World!");
    }
    
    [Command("connect")]
    public async Task ConnectCommand(CommandContext ctx, string host, int port)
    {
        await ctx.Player.SendMessageAsync("Connecting you to " + host + ":" + port + "...");
        try
        {
            await ctx.Player.Connect(host, (uint)port);
            await ctx.Player.SendMessageAsync("Connected!");
        }catch(Exception e)
        {
            await ctx.Player.SendMessageAsync("Failed to connect: " + e.Message);
            Log.Error(e, "Failed to connect");
        }

    }

    [TabComplete("connect")]
    public Task<string[]> ConnectTabComplete(TabCompleteContext ctx)
    {
        if (ctx.ArgIndex == 0)
        {
            return Task.FromResult(new[] {"localhost"});
        }else if(ctx.ArgIndex == 1)
        {
            return Task.FromResult(new[] {"25565"});
        }
        return Task.FromResult(new string[0]);
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

    [Command("test_command")]
    public async Task TestCommand(CommandContext ctx, string test, int test1)
    {

    }

    [TabComplete("test_command")]
    public async Task<string[]> TestCommandTabComplete(TabCompleteContext ctx)
    {
        return new[] {"test" + ctx.ArgIndex};
    }
    
    [Command("bossbar")]
    public async Task BossbarCommand(CommandContext ctx, string text, int percent)
    {
        if (ctx.Player.Bossbar is null)
        {
            await ctx.Player.SendBossbarAsync(x =>
            {
                x.Title = text;
                x.Health = percent;
                x.Color = BossbarColor.Green;
                x.Division = BossbarDivision.NoDivision;
            });
            await ctx.Player.SendMessageAsync("Bossbar created!");
        }else
        {
            await ctx.Player.Bossbar.ModifyAsync(x =>
            {
                x.Health = percent;
                x.Title = text;
            });
            await ctx.Player.SendMessageAsync("Bossbar updated!");
        }
    }
    
    [Command("bossbar_remove")]
    public async Task BossbarRemoveCommand(CommandContext ctx)
    {
        if (ctx.Player.Bossbar is null)
        {
            await ctx.Player.SendMessageAsync("No bossbar to remove!");
        }else
        {
            await ctx.Player.RemoveBossbarAsync();
            await ctx.Player.SendMessageAsync("Bossbar removed!");
        }
    }
    
}