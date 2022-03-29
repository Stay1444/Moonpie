using System.Threading.Tasks;
using Moonpie.Entities.Models.Events;

namespace Moonpie.Plugins;

public abstract class BaseEventListener
{
    public virtual Task OnPlayerPing(PlayerPingEventArgs e) => Task.CompletedTask;
}