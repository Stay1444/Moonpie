using Moonpie.Entities;

namespace Moonpie.Plugins;

public class TabCompleteContext
{
    public TabCompleteContext(CommandInfo command, Player player, Moonpie proxy, MoonpiePlugin plugin, string[] args)
    {
        Command = command;
        Player = player;
        Proxy = proxy;
        Plugin = plugin;
        Args = args;
    }
    public string[] Args { get; set; }
    public CommandInfo Command { get; init; }
    public Player Player { get; init; }
    public Moonpie Proxy { get; init; }
    public MoonpiePlugin Plugin { get; init; }
}