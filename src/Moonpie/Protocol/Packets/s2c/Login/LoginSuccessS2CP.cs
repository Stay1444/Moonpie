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
using Moonpie.Protocol.Protocol;
using Moonpie.Utils.Protocol;
using Serilog;

namespace Moonpie.Protocol.Packets.s2c.Login;

[PacketType(PacketTypes.S2C.LOGIN_LOGIN_SUCCESS)]
public class LoginSuccessS2CP : IS2CPacket
{
    public JavaUUID Uuid { get; set; }
    public string? Name { get; set; }
    
    public void Read(InByteBuffer buffer)
    {
        Uuid = buffer.Version < ProtocolVersion.v20w12a ? buffer.ReadUUIDString() : buffer.ReadUUID();
        Name = buffer.ReadString();
    }

    public void Write(OutByteBuffer buffer)
    {  
        if (buffer.Version < ProtocolVersion.v20w12a)
        {
            buffer.WriteUUIDString(Uuid);
        }
        else
        {
            buffer.WriteUUID(Uuid);
        }
        
        buffer.WriteString(Name ?? "");
    }

    public async Task Handle(PacketHandleContext handler)
    {
        
        await handler.Transport.PlayerTransport.Connection.WritePacketAsync(new CompressionSetS2CP()
        {
            Threshold = handler.Proxy.Configuration.CompressionThreshold
        });
        handler.Transport.PlayerTransport.Connection.CompressionThreshold = handler.Proxy.Configuration.CompressionThreshold;
        
        handler.Transport.ServerTransport!.Connection.State = ProtocolState.Play;
        handler.Transport.PlayerTransport.Connection.State = ProtocolState.Play;
    }
}