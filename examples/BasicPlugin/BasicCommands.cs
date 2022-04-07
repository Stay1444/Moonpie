using Moonpie.Entities;
using Moonpie.Entities.Enums;
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
        
        if (ctx.Player.BossbarManager.Bossbars.Count == 0)
        {
            await ctx.Player.BossbarManager.CreateBossbar(new BossbarBuilder().WithTitle(text)
                .WithColor(BossbarColor.Blue)
                .WithDivision(BossbarDivision.Notches6)
                .WithHealth(percent / 100f));
            
            await ctx.Player.SendMessageAsync("Bossbar created!");

            return;
        }

        var bossbar = ctx.Player.BossbarManager.Bossbars.FirstOrDefault(x => x.Value.Owner == BossbarOwner.Moonpie).Value;

        await bossbar.ModifyAsync(x =>
        {
            x.Title = text;
            x.Health = percent / 100f;
        });
        
        await ctx.Player.SendMessageAsync("Bossbar modified!");
    }
    
    [Command("bossbar_remove")]
    public async Task BossbarRemoveCommand(CommandContext ctx)
    {
        if (!ctx.Player.BossbarManager.Bossbars.Any())
        {
            await ctx.Player.SendMessageAsync("No bossbar to remove!");
        }else
        {
            foreach (var bossbar in ctx.Player.BossbarManager.Bossbars)
            {
                await bossbar.Value.DeleteAsync();
                await ctx.Player.SendMessageAsync($"Bossbar {bossbar.Key} removed!");
            }
        }
    }

    [Command("test_title")]
    public async Task TitlePacket(CommandContext ctx, string text, string? subtext)
    {
        var titleBuilder = new TitleBuilder()
            .WithText(text)
            .WithSubtitle(subtext)
            .WithFadeIn(TimeSpan.FromSeconds(1))
            .WithDuration(TimeSpan.FromSeconds(5))
            .WithFadeOut(TimeSpan.FromSeconds(1));
        
        await ctx.Player.SendTitleAsync(titleBuilder);
        
        await ctx.Player.SendMessageAsync("Title sent!");
    }
    
}