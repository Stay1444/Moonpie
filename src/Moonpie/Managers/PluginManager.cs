using System.Reflection;
using Moonpie.Entities;
using Moonpie.Entities.Models.Events;
using Moonpie.Plugins;
using Moonpie.Plugins.Attributes;
using Serilog;

namespace Moonpie.Managers;

public class PluginManager
{

    public PluginManager(Moonpie proxy)
    {
        _proxy = proxy;
    }
    private List<MoonpiePlugin>? plugins;
    public IReadOnlyList<MoonpiePlugin> Plugins => plugins!;
    private Moonpie _proxy;

    internal void Load()
    {
        if (plugins != null)
        {
            throw new Exception("PluginManager already loaded");            
        }
        
        plugins = new List<MoonpiePlugin>();
        
        if (!Directory.Exists("plugins"))
        {
            Directory.CreateDirectory("plugins");
        }
        
        foreach (var file in Directory.GetFiles("plugins", "*.dll"))
        {
            try
            {
                var assembly = Assembly.LoadFrom(file);
                foreach (var type in assembly.GetTypes())
                {
                    if (type.IsSubclassOf(typeof(MoonpiePlugin)))
                    {
                        try
                        {
                            Log.Information("Loading plugin {name}", type.Assembly.GetName().Name);
                            var plugin = (MoonpiePlugin?) Activator.CreateInstance(type);
                            if (plugin is null) continue;
                            plugins.Add(plugin);
                            plugin.Proxy = _proxy;
                            plugin.OnLoad();
                        }catch(Exception e)
                        {
                            Log.Error(e, "Failed to load plugin {name}", type.Assembly.GetName().Name);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error(e, "Failed to load plugin from {file}", file);
            }
        }
    }
    
    internal async Task<T?> TriggerEventAsync<T>(T args) where T : MoonpieEventArgs
    {
        foreach (var plugin in plugins!)
        {

            foreach (var listener in plugin.Listeners)
            {
                foreach (var methods in listener.GetType().GetMethods())
                {
                    if (!methods.GetParameters().Any()) continue;
                    if (methods.GetParameters().First().ParameterType == args.GetType())
                    {
                        try
                        {
                            await ((Task?) methods.Invoke(listener, new object[] {args}))!;
                            if (args.Handled) return args;
                        }
                        catch (Exception e)
                        {
                            Log.Error(e, "Failed to invoke {method}", methods.Name);
                        }
                    }
                }
            }
        }
        return args;
    }

    internal Task<bool> TriggerCommandAsync(Player player, string text)
    {
        string commandName = text.Split(' ')[0];
        string[] args = text.Split(' ').Skip(1).ToArray();
        commandName = commandName.Substring(1);
        foreach (var plugin in plugins!)
        {
            foreach (var commandInfo in plugin.Commands)
            {
                if (commandInfo.Name == commandName)
                {
                    _ = RunComandAsync(commandInfo, args, player);
                    return Task.FromResult(true);
                }
            }
        }
        return Task.FromResult(false);
    }

    private async Task RunComandAsync(CommandInfo command, string[] args, Player player)
    {
        try
        {
            var context = new CommandContext(command, player, _proxy,
                Plugins.FirstOrDefault(p => p.Commands.Any(c => c.GetType() == command.Method.GetType()))!);
            var methodParameters = command.Method.GetParameters();
            
            object[] parameters = new object[methodParameters.Length];
            
            for (int i = 0; i < methodParameters.Length; i++)
            {
                if (methodParameters[i].ParameterType == typeof(CommandContext))
                {
                    parameters[i] = context;
                    continue;
                }

                if (methodParameters[i].ParameterType == typeof(int))
                {
                    parameters[i] = int.Parse(args[i - 1]);
                    continue;
                }
                
                if (methodParameters[i].ParameterType == typeof(string))
                {
                    parameters[i] = args[i - 1];
                    continue;
                }
            }
            
            await (Task) command.Method.Invoke(command.Module, parameters)!;
            
        }catch(Exception e)
        {
            Log.Error(e, "Failed to run command {command}", command.Name);
        }
    }
}