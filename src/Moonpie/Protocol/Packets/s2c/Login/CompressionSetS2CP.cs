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
using Moonpie.Protocol.Network;
using Moonpie.Protocol.Packets.c2s;
using Moonpie.Protocol.Protocol;

namespace Moonpie.Protocol.Packets.s2c.Login;

[PacketType(PacketTypes.S2C.LoginCompressionSet)]
public class CompressionSetS2CP : IS2CPacket
{
    public int Threshold { get; set; }

    public void Read(InByteBuffer buffer)
    {
        Threshold = buffer.ReadVarInt();
    }

    public void Write(OutByteBuffer buffer)
    {
        buffer.WriteVarInt(Threshold);
    }
    
    public Task Handle(PacketHandleContext context)
    {
        context.Cancel();
        context.Transport.ServerTransport!.Connection.CompressionThreshold = Threshold;
        Console.WriteLine($"Compression threshold set to {Threshold}");
        return Task.CompletedTask;
    }
}