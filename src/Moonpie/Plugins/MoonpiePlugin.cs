#region Copyright
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

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Moonpie.Plugins.Attributes;
using Serilog;

namespace Moonpie.Plugins;

public abstract class MoonpiePlugin
{
    public Moonpie Proxy { get; internal set; }
    private List<BaseEventListener> _listeners = new List<BaseEventListener>();
    public IReadOnlyCollection<BaseEventListener> Listeners => _listeners;
    
    private List<CommandInfo> _commands = new List<CommandInfo>();
    public IReadOnlyCollection<CommandInfo> Commands => _commands;
    
    public abstract Task OnLoad();
    public abstract Task OnUnload();
    
    protected Task RegisterCommands<T>(T instance) where T : BaseCommandModule
    {
        foreach (var method in instance.GetType().GetMethods())
        {
            if (method.GetCustomAttribute<Command>() is null) continue;

            if (!method.GetParameters().Any())
            {
                Log.Error("Command {0} has no CommandContext paramenter.", method.Name);
                continue;
            }

            if (method.GetParameters().First().ParameterType != typeof(CommandContext))
            {
                Log.Error("Command {0} has an invalid CommandContext paramenter.", method.Name);
                continue;
            }
            
            var commandInfo = new CommandInfo(method, instance);
            foreach (var method2 in instance.GetType().GetMethods())
            {
                if (method2.GetCustomAttribute<TabComplete>() is null) continue;
                var tabCompleteAttribute = method2.GetCustomAttribute<TabComplete>();
                if (tabCompleteAttribute!.Name != commandInfo.Name) continue;

                if (method2.GetParameters().Length != 1)
                {
                    Log.Error("TabComplete {0} has an invalid TabCompleteContext paramenter.", method2.Name);
                    continue;
                }
                
                if (method2.GetParameters()[0].ParameterType != typeof(TabCompleteContext))
                {
                    Log.Error("TabComplete {0} has an invalid TabCompleteContext paramenter.", method2.Name);
                    continue;
                }

                if (method2.ReturnType != typeof(Task<string[]>))
                {
                    Log.Error("TabComplete {0} has an invalid return type. Expected: {1}", method2.Name, typeof(Task<string[]>));
                    continue;
                }

                if (method.GetParameters().Length < 2)
                {
                    Log.Error("Command {0} does not have any parameters for tab completion.", method.Name);
                    continue;
                }

                commandInfo.TabCompleteInfo = new TabCompleteInfo(commandInfo, method2, instance);
                break;
            }
            _commands.Add(commandInfo);
        }
        return Task.CompletedTask;
    }
    
    protected Task RegisterEvents<T>(T instance) where T : BaseEventListener
    {
        _listeners.Add(instance);
        return Task.CompletedTask;
    }
    
}
