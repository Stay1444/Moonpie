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

namespace Moonpie.Protocol.Packets.c2s.Login;

[PacketType(PacketTypes.C2S.LOGIN_PLUGIN_RESPONSE)]
public class LoginPluginResponseC2SP : IC2SPacket
{
    public VarInt MessageId { get; set; }
    public bool Successful { get; set; }
    public byte[]? Data { get; set; }
    
    public void Read(InByteBuffer buffer)
    {
        MessageId = buffer.ReadVarInt();
        Successful = buffer.ReadBoolean();
        if (buffer.Position < buffer.Length)
        {
            Data = buffer.ReadBytes(buffer.Length - buffer.Position);
        }
    }

    public void Write(OutByteBuffer buffer)
    {
        buffer.WriteVarInt(MessageId);
        buffer.WriteBoolean(Successful);
        if (Data != null)
        {
            buffer.WriteBytes(Data);
        }
    }
}