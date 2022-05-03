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

using System.Text.Json;
using Moonpie.Protocol.Network;
using Moonpie.Protocol.Protocol;
using Moonpie.Utils.Protocol;

namespace Moonpie.Protocol.Packets.s2c.Play;

[PacketType(PacketTypes.S2C.PlayChatMessage)]
public class ChatMessageS2CP : IS2CPacket
{
    public ChatComponent? Message { get; set; } = ChatComponent.Empty;
    public string? Translate { get; set; }
    public ChatTextPositions Position { get; set; } = ChatTextPositions.SystemMessage;
    public JavaUUID? Sender { get; set; } = null;
    public void Read(InByteBuffer buffer)
    {
        var json = buffer.ReadString();
        if (json.Contains("translate"))
        {
            Translate = json;
        }else
        {
            Message = ChatComponent.FromJson(json);
        }
        
        if (buffer.Version >= ProtocolVersion.v14w04a)
        {
            Position = (ChatTextPositions) buffer.ReadByte();
            if (buffer.Version >= ProtocolVersion.v20w21a)
            {
                Sender = buffer.ReadUUID();
            }
        }
    }

    public void Write(OutByteBuffer buffer)
    {
        if (Translate != null)
        {
            buffer.WriteString(Translate);
        }
        else
        {
            var json = JsonSerializer.Serialize(Message);
            buffer.WriteString(json);
        }
        if (buffer.Version >= ProtocolVersion.v14w04a)
        {
            buffer.WriteByte((byte) Position);
            if (buffer.Version >= ProtocolVersion.v20w21a)
            {
                buffer.WriteUUID(Sender);
            }
        }
    }

    public enum ChatTextPositions
    {
        ChatBox,
        SystemMessage,
        AboveHotbar,
    }

}