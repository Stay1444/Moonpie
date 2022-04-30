using Moonpie.Plugins.Attributes;
using Moonpie.Protocol.Protocol;

namespace Moonpie.Plugins.Internal;

public class InternalCommands : BaseCommandModule
{
    
    [Command("moonpie")]
    public async Task Moonpie(CommandContext ctx)
    {
        await ctx.Player.SendMessageLinesAsync(
            ChatComponent.From(ChatColor.Blue, ChatColor.Bold, "Moonpie"),
            ChatComponent.From(ChatColor.DarkGray, "| ", ChatColor.Gray, "This network is using Moonpie."),
            ChatComponent.From(ChatColor.DarkGray, "| ", ChatColor.Gray, "Version: ", ctx.Player.Version),
            ChatComponent.From(ChatColor.DarkGray, "| ", ChatColor.Gray, "More info: ", ChatColor.Aqua, new ChatComponent.ClickEvent("open_url", "https://github.com/Stay1444/Moonpie"))
        );
    }
}