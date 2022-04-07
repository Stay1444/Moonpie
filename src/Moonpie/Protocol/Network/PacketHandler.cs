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

using System.Diagnostics;
using System.Threading.Tasks;
using Moonpie.Entities;
using Moonpie.Protocol.Packets;
using Serilog;

namespace Moonpie.Protocol.Network;

public class PacketHandler
{
    public PacketHandler(Moonpie proxy, Player player, TransportManager transport)
    {
        Proxy = proxy;
        Player = player;
        Transport = transport;
    }

    public Moonpie Proxy { get; private set; }
    public Player Player { get; private set; }
    public TransportManager Transport { get; private set; }
    
    public async Task<bool> Handle(IPacket packet)
    {
        try
        {
            var ctx = new PacketHandleContext(Proxy, Player, Transport, this);
            var stopwatch = Stopwatch.StartNew();
            var earlyReturn = ctx.TExitEarly.Task;
            var handleTask = packet.Handle(ctx);
            await Task.WhenAny(earlyReturn, handleTask);
            stopwatch.Stop();
            if (stopwatch.ElapsedMilliseconds > 100)
            {
                Log.Warning("Packet {0} took {1}ms to handle", packet.GetType().Name, stopwatch.ElapsedMilliseconds);
            }
            return ctx.IsCanceled;
        }catch(Exception e)
        {
            Console.WriteLine(e);
        }

        return false;
    }
}