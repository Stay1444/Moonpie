using Moonpie.Plugins;

namespace BasicPlugin;

public class BasicPlugin : MoonpiePlugin
{
    public override Task OnLoad()
    {
        Console.WriteLine("BasicPlugin loaded");
        RegisterEvents<BasicEventListener>(new BasicEventListener());
        RegisterCommands<BasicCommands>(new BasicCommands());
        return Task.CompletedTask;
    }

    public override Task OnUnload()
    {
        Console.WriteLine("BasicPlugin unloaded");
        return Task.CompletedTask;
    }
}