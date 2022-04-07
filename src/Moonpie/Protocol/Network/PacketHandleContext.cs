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

using System.Threading.Tasks;
using Moonpie.Entities;

namespace Moonpie.Protocol.Network;

public class PacketHandleContext
{
    public PacketHandleContext(Moonpie proxy, Player player, TransportManager transport, PacketHandler packetHandler)
    {
        Proxy = proxy;
        Player = player;
        Transport = transport;
        PacketHandler = packetHandler;
    }

    public Moonpie Proxy { get; init; }
    public Player Player { get; init; }
    public TransportManager Transport { get; init; }
    public PacketHandler PacketHandler { get; init; }
    public bool IsCanceled { get; private set; }
    public void Cancel()
    {
        IsCanceled = true;
    }

    internal TaskCompletionSource TExitEarly { get; set; } = new TaskCompletionSource(); 
    public void ExitEarly()
    {
        TExitEarly.SetResult();
    }
}