using Moonpie.Plugins;
using Serilog;

namespace Moonpie.InternalPlugin;

public class InternalPlugin : MoonpiePlugin
{
    public override Task OnLoad()
    {
        Log.Information("InternalPlugin loaded");
        RegisterCommands(new InternalCommands());
        return Task.CompletedTask;
    }

    public override Task OnUnload()
    {
        return Task.CompletedTask;
    }
}