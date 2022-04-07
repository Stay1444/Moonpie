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

using Moonpie.Entities;

namespace Moonpie.Plugins;

public class TabCompleteContext
{
    public TabCompleteContext(CommandInfo command, Player player, Moonpie proxy, MoonpiePlugin plugin, string currentArg, string[] fullArgs, int argIndex)
    {
        Command = command;
        Player = player;
        Proxy = proxy;
        Plugin = plugin;
        CurrentArg = currentArg;
        FullArgs = fullArgs;
        ArgIndex = argIndex;
    }
    public CommandInfo Command { get; init; }
    public Player Player { get; init; }
    public Moonpie Proxy { get; init; }
    public MoonpiePlugin Plugin { get; init; }
    
    public string CurrentArg { get; set; }
    public string[] FullArgs { get; init; }
    public int ArgIndex { get; init; }
}