#region License
// Moonpie
// 
// Copyright (c) 2022 Stay
// 
// MIT License
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
#endregion

using System.Reflection;
using Moonpie.Entities;
using Moonpie.Entities.Models.Events;
using Moonpie.Plugins;
using Moonpie.Plugins.Internal;
using Moonpie.Protocol.Packets.s2c.Play;
using Moonpie.Protocol.Protocol;
using Serilog;

namespace Moonpie.Managers;

public class PluginManager
{
    public IReadOnlyList<MoonpiePlugin> Plugins => _plugins!;
    
    private List<MoonpiePlugin>? _plugins;
    private readonly Moonpie _proxy;
    
    private const string PluginDirectory = "plugins";
    
    internal PluginManager(Moonpie proxy)
    {
        _proxy = proxy;
    }

    internal void Load()
    {
        if (_plugins != null)
        {
            throw new Exception("PluginManager already loaded");            
        }
        
        _plugins = new List<MoonpiePlugin>();
        
        if (!Directory.Exists(PluginDirectory))
        {
            Directory.CreateDirectory(PluginDirectory);
        }

        if (!_proxy.Configuration.Others.DisableInternalCommands)
        {
            var internalPlugin = new InternalPlugin();
            _plugins.Add(internalPlugin);
            internalPlugin.OnLoad();
        }

        // Loop through all files ending in .dll in PluginsDirectory
        foreach (var file in Directory.GetFiles(PluginDirectory, "*.dll"))
        {
            try
            {
                var assembly = Assembly.LoadFrom(file);
                
                // Loop through each type in the assembly until we find one that is a subclass of MoonpiePlugin
                foreach (var type in assembly.GetTypes())
                {
                    if (type.IsSubclassOf(typeof(MoonpiePlugin)))
                    {
                        try
                        {
                            Log.Information("Loading plugin {name}", type.Assembly.GetName().Name);
                            
                            var plugin = (MoonpiePlugin?) Activator.CreateInstance(type); // Create an instance of the plugin. ToDo: Service provider?

                            if (plugin is null) continue;
                            
                            _plugins.Add(plugin);
                            
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
    
    //ToDo: Optimize this, currently its using reflection every time an event is fired. I don't think this is a big deal, but it would be nice to optimize this.
    internal async Task<T?> TriggerEventAsync<T>(T args) where T : MoonpieEventArgs
    {
        // Loop through all the plugins loaded
        foreach (var plugin in _plugins!)
        {
            // Loop through the plugin event listeners
            foreach (var listener in plugin.Listeners)
            {
                // Get all the methods 
                var methods = listener.GetType().GetMethods();
                
                // Loop through all the methods
                foreach (var method in methods)
                {
                    var baseDefinition = method.GetBaseDefinition(); // Get the base definition of the method
                    
                    if (baseDefinition.DeclaringType != typeof(BaseEventListener)) continue; // If the method is not a listener, skip it
                    
                    if (!method.GetParameters().Any()) continue; // If the method has no parameters, skip it
                    
                    if (method.GetParameters().First().ParameterType == args.GetType()) // If the method's first parameter is the same type as the event args, execute it
                    {
                        try
                        {
                            await ((Task?) method.Invoke(listener, new object[] {args}))!;
                            
                            if (args.Handled) return args;
                        }
                        catch (Exception e)
                        {
                            Log.Error(e, "Failed to invoke {method}", method.Name);
                        }
                    }
                }
            }
        }
        return args;
    }

    internal async Task<bool> TriggerCommandAsync(Player player, string text)
    {
        string commandName = text.Split(' ')[0]; // Get the command name
        
        string[] args = text.Split(' ').Skip(1).ToArray(); // Get the arguments (skip the command name)
        
        commandName = commandName.Substring(1); // Remove the slash from the command name
        
        if (_proxy.Configuration.Others.DisabledCommands.Contains(commandName)) 
        {
            await player.SendMessageAsync("Unknown command. Type \"/help\" for help.");
            return true;
        }
        
        foreach (var plugin in _plugins!)
        {
            foreach (var commandInfo in plugin.Commands)
            {
                if (commandInfo.Name == commandName)
                {
                    _ = RunComandAsync(commandInfo, args, player);
                    return true;
                }
            }
        }
        return false;
    }

    internal bool DoesCommandExist(string commandName)
    {
        foreach (var plugin in _plugins!)
        {
            foreach (var commandInfo in plugin.Commands)
            {
                if (commandInfo.Name == commandName)
                {
                    return true;
                }
            }
        }
        return false;
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

    //ToDo: This is a fucking mess and needs to be cleaned up
    internal Dictionary<string, KeyValuePair<DeclareCommandsS2CP.CommandNode, List<DeclareCommandsS2CP.CommandNode>?>> RegisterCommands(Player player)
    {
        if (_plugins is null) return new Dictionary<string, KeyValuePair<DeclareCommandsS2CP.CommandNode, List<DeclareCommandsS2CP.CommandNode>?>>();
        var list = new Dictionary<string, KeyValuePair<DeclareCommandsS2CP.CommandNode, List<DeclareCommandsS2CP.CommandNode>?>>();

        bool AreNextParametersRequired(ParameterInfo[] parameters, int startIndex)
        {
            for (int i = startIndex; i < parameters.Length; i++)
            {
                if (parameters[i].IsOptional) return false;
            }
            
            return true;
        }
        
        foreach (var plugin in _plugins)
        {
            foreach (var commandInfo in plugin.Commands)
            {
                if (commandInfo.Name is null) continue;
                var parameters = commandInfo.Method.GetParameters().Where(x => x.ParameterType != typeof(CommandContext)).ToArray();
                var literal = DeclareCommandsS2CP.CommandLiteralNode.Create(commandInfo.Name!,
                    parameters.Length == 0);
                list.Add(literal.Name, new KeyValuePair<DeclareCommandsS2CP.CommandNode, List<DeclareCommandsS2CP.CommandNode>?>(literal, null));
                if (parameters.Length > 0)
                {
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        var parameter = parameters[i];
                        if (parameter.ParameterType == typeof(string))
                        {
                            var agument = DeclareCommandsS2CP.CommandArgumentNode.Create(parameter.Name ?? "string", !AreNextParametersRequired(parameters, i), "brigadier:string",
                                DeclareCommandsS2CP.CommandArgumentNode.SuggestionTypes.ASK_SERVER, new List<object>
                                {
                                    new VarInt(0)
                                });

                            if (list[commandInfo.Name].Value is null)
                            {
                                list[commandInfo.Name] = new KeyValuePair<DeclareCommandsS2CP.CommandNode, List<DeclareCommandsS2CP.CommandNode>?>(
                                    literal, new List<DeclareCommandsS2CP.CommandNode> {agument});
                            }else
                            {
                                list[commandInfo.Name].Value!.Add(agument);
                            }
                            
                            continue;
                        }
                        
                        if (parameter.ParameterType == typeof(int))
                        {
                            var agument = DeclareCommandsS2CP.CommandArgumentNode.Create(parameter.Name ?? "int", true, "brigadier:integer",
                                DeclareCommandsS2CP.CommandArgumentNode.SuggestionTypes.ASK_SERVER, new List<object>
                                {
                                    (byte)0x00
                                });

                            if (list[commandInfo.Name].Value is null)
                            {
                                list[commandInfo.Name] = new KeyValuePair<DeclareCommandsS2CP.CommandNode, List<DeclareCommandsS2CP.CommandNode>?>(
                                    literal, new List<DeclareCommandsS2CP.CommandNode> {agument});
                            }else
                            {
                                list[commandInfo.Name].Value!.Add(agument);
                            }
                            
                            continue;
                        }
                    }
                }
                
            }
        }
        
        return list;
    }
    
    public T? GetInstance<T>() where T : MoonpiePlugin
    {
        return _plugins!.FirstOrDefault(x => x is T) as T;
    }

    //ToDo: Same as the RegisterCommands method. This is a temporary solution.
    internal Task<bool> HandleAutoCompletionAsync(Player player, int transactionId, string text)
    {
        string[] split = text.Substring(1).Split(' ');
        if (split.Length == 0) return Task.FromResult(false);
        if (!DoesCommandExist(split[0])) return Task.FromResult(false);
        if (GetCommandInfo(split[0])!.TabCompleteInfo is null) return Task.FromResult(false);
        _ = RunAutoCompletionAsync(player, transactionId, split[0], split.Skip(1).ToArray());
        return Task.FromResult(true);
    }
    
    private CommandInfo? GetCommandInfo(string commandName)
    {
        foreach (var plugin in _plugins!)
        {
            foreach (var commandInfo in plugin.Commands)
            {
                if (commandInfo.Name == commandName) return commandInfo;
            }
        }

        return null;
    }

    private MoonpiePlugin? GetPlugin(CommandInfo info)
    {
        foreach (var plugin in _plugins!)
        {
            foreach (var commandInfo in plugin.Commands)
            {
                if (commandInfo.Name == info.Name) return plugin;
            }
        }
        
        return null;
    }

    //ToDo: Wow. This is a temporary solution.
    private async Task RunAutoCompletionAsync(Player player, int transactionId, string command, string[] ogargs)
    {
        if (ogargs.Any() && ogargs[0] == "")
        {
            ogargs = new string[0];
        }
        
        var args = ogargs;//ogargs.Where( x=> !string.IsNullOrEmpty(x)).ToArray();
        var commandInfo = GetCommandInfo(command)!;
        var tabCompleteInfo = commandInfo.TabCompleteInfo!;
        var plugin = GetPlugin(commandInfo)!;
        string currentArg = args.Length > 0 ? args[^1] : "";
        int argIndex = args.Length > 0 ? args.Length - 1 : 0;
        
        var context = new TabCompleteContext(commandInfo, player, this._proxy, plugin, currentArg, args, argIndex);
        try
        {
            if (tabCompleteInfo.Method.Invoke(tabCompleteInfo.Module, new object[] {context})! is not Task<string[]> result) return;
            var text = await result!;
            TabCompleteS2CP tabCompleteS2CP = new TabCompleteS2CP();
            tabCompleteS2CP.TransactionId = transactionId;
            tabCompleteS2CP.StartIndex = command.Length + string.Join(" ", args.Take(args.Length - 1)).Length + 1 + args.Length + (ogargs.Any() ? 0 : 1);
            if (text.Any() && args.Any())
            {
                for (int i = 0; i < text[0].Length && i < args[^1].Length; i++)
                {
                    if (text[0][i] == args[^1][i])
                    {
                        tabCompleteS2CP.Length++;
                    }else
                    {
                        break;
                    }
                }
            }            
            
            foreach (var textResult in text)
            {
                tabCompleteS2CP.Matches.Add(new TabCompleteS2CP.Match()
                {
                    MatchText = textResult
                });
            }

            await player.Transport.PlayerTransport.Connection.WritePacketAsync(tabCompleteS2CP);
            Console.WriteLine($"{player.Username} tab completed {command} {string.Join(" ", args)}");
        }
        catch (Exception e)
        {
            Log.Error(e, "Error while running tab completion for command " + command);
        }
    }
}