using Moonpie.Entities;

namespace Moonpie.Plugins;

public class CommandContext
{
    public CommandContext(CommandInfo command, Player player, Moonpie proxy, MoonpiePlugin plugin)
    {
        Command = command;
        Player = player;
        Proxy = proxy;
        Plugin = plugin;
    }

    public CommandInfo Command { get; init; }
    public Player Player { get; init; }
    public Moonpie Proxy { get; init; }
    public MoonpiePlugin Plugin { get; init; }
}