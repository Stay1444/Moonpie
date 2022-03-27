using System.Reflection;
using Moonpie.Plugins.Attributes;

namespace Moonpie.Plugins;

public class CommandInfo
{
    public string? Name { get; private set; }
    public BaseCommandModule Module { get; private set; }
    public MethodInfo Method { get; private set; }
    internal CommandInfo(MethodInfo info, BaseCommandModule module)
    {
        this.Module = module;
        this.Method = info;
        var commandAttribute = info.GetCustomAttribute<Command>();
        if (commandAttribute is not null)
        {
            Name = commandAttribute.Name;
        }
    }
}