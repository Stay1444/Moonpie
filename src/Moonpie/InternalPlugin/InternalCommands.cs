using Moonpie.Plugins;
using Moonpie.Plugins.Attributes;
using Moonpie.Protocol.Protocol;

namespace Moonpie.InternalPlugin;

public class InternalCommands : BaseCommandModule
{
    
    [Command("moonpie")]
    public async Task Moonpie(CommandContext ctx)
    {
        await ctx.Player.SendMessageAsync(
            ChatComponent.Parse("\n &9Moonpie Proxy  \n&8│ &fThis network is using Moonpie. \n&8│ &fIn its version: &3" + ctx.Player.Version + "\n&8│ &fMore info: &dhttps://github.com/Stay1444/Moonpie\n", '&'));
    }
    
    
    [Command("whereami")]
    public async Task Whereami(CommandContext ctx)
    {
        await ctx.Player.SendMessageAsync(
            ChatComponent.Parse("&7You are on proxy: &3proxy01\n&7On the server: " + ctx.Player.Transport.ServerTransport!.Connection.RemoteEndPoint + "\n&7With coords: &9X &7-> &30 &9Y &7-> &30 &9Z &7-> &30", '&'));
    }
    
}