using Moonpie.Entities.Models;
using Moonpie.Entities.Models.Events;
using Moonpie.Plugins;
using Moonpie.Protocol.Protocol;

namespace BasicPlugin;

public class BasicEventListener : BaseEventListener
{
    public override Task OnPlayerPing(PlayerPingEventArgs e)
    {
        e.Response = new ServerStatusResponseBuilder();
        e.Response.WithDescription(new ChatComponent("Basic MOTD"));
        e.Response.WithPlayers(1444, 1444);
        e.Response.WithVersion(e.Version);
        return Task.CompletedTask;
    }
}