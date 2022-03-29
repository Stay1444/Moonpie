using System.Reflection;
using Moonpie.Plugins.Attributes;

namespace Moonpie.Plugins;

public class TabCompleteInfo
{
    public CommandInfo Command { get; private set; }
    public BaseCommandModule Module { get; private set; }
    public MethodInfo Method { get; private set; }
    internal TabCompleteInfo(CommandInfo command, MethodInfo info, BaseCommandModule module)
    {
        this.Module = module;
        this.Method = info;
        this.Command = command;
    }
}