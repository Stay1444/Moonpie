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

using Moonpie.Protocol.Network;
using Moonpie.Protocol.Protocol;

namespace Moonpie.Protocol.Packets.c2s;

public class GenericC2SP : IC2SPacket
{
    public PacketTypes.C2S Type { get; }
    private byte[] _data = Array.Empty<byte>();

    public GenericC2SP(PacketTypes.C2S type)
    {
        Type = type;
    }

    public void Log()
    {
        Console.WriteLine("GenericC2SP");
    }
    
    public void Read(InByteBuffer buffer)
    {
        _data = buffer.ReadBytes(buffer.Length - buffer.Position);
    }

    public void Write(OutByteBuffer buffer)
    {
        buffer.WriteBytes(_data);
    }
}