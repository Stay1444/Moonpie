using Moonpie.Plugins;
using Serilog;

namespace Moonpie.Plugins.Internal;

public class InternalPlugin : MoonpiePlugin
{
    public override Task OnLoad()
    {
        RegisterCommands(new InternalCommands());
        return Task.CompletedTask;
    }

    public override Task OnUnload()
    {
        return Task.CompletedTask;
    }
}