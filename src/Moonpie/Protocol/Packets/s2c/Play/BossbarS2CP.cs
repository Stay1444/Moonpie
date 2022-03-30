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

using Moonpie.Protocol.Network;
using Moonpie.Protocol.Protocol;
using Moonpie.Utils.Protocol;

namespace Moonpie.Protocol.Packets.s2c.Play;

[PacketType(PacketTypes.S2C.PLAY_BOSS_BAR)]
public class BossbarS2CP : IS2CPacket
{
    public enum BossbarAction
    {
        Add,
        Remove,
        UpdateHealth,
        UpdateTitle,
        UpdateStyle,
        UpdateFlags
    }

    public enum BossbarColor
    {
        Pink,
        Blue,
        Red,
        Green,
        Yellow,
        Purple,
        White
    }

    public enum BossbarDivision
    {
        NoDivision,
        Notches6,
        Notches10,
        Notches12,
        Notches20
    }
    
    public JavaUUID Uuid { get; set; }
    public BossbarAction Action { get; set; }
    public ChatComponent? Title { get; set; }
    public float? Health { get; set; }
    public BossbarColor? Color { get; set; }
    public BossbarDivision? Division { get; set; }
    public byte? Flags { get; set; }
    
    public void Read(InByteBuffer buffer)
    {
        Uuid = buffer.ReadUUID();
        Action = (BossbarAction)buffer.ReadVarInt();
        
        switch (Action)
        {
            case BossbarAction.Add:
                Title = buffer.ReadChatComponent();
                Health = buffer.ReadFloat();
                Color = (BossbarColor)buffer.ReadVarInt();
                Division = (BossbarDivision)buffer.ReadVarInt();
                Flags = buffer.ReadByte();
                break;
            case BossbarAction.Remove:
                // nothing
                break;
            case BossbarAction.UpdateHealth:
                Health = buffer.ReadFloat();
                break;
            case BossbarAction.UpdateTitle:
                Title = buffer.ReadChatComponent();
                break;
            case BossbarAction.UpdateStyle:
                Color = (BossbarColor)buffer.ReadVarInt();
                Division = (BossbarDivision)buffer.ReadVarInt();
                break;
            case BossbarAction.UpdateFlags:
                Flags = buffer.ReadByte();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

    }

    public void Write(OutByteBuffer buffer)
    {
        buffer.WriteUUID(Uuid);
        buffer.WriteVarInt((int)Action);
        
        switch (Action)
        {
            case BossbarAction.Add:
                buffer.WriteChatComponent(Title ?? ChatComponent.Empty);
                buffer.WriteFloat(Health ?? 0);
                buffer.WriteVarInt((int)(Color ?? 0));
                buffer.WriteVarInt((int)(Division ?? 0));
                buffer.WriteByte(Flags ?? 0);
                break;
            case BossbarAction.Remove:
                // nothing
                break;
            case BossbarAction.UpdateHealth:
                buffer.WriteFloat(Health ?? 0);
                break;
            case BossbarAction.UpdateTitle:
                buffer.WriteChatComponent(Title ?? ChatComponent.Empty);
                break;
            case BossbarAction.UpdateStyle:
                buffer.WriteVarInt((int)(Color ?? 0));
                buffer.WriteVarInt((int)(Division ?? 0));
                break;
            case BossbarAction.UpdateFlags:
                buffer.WriteByte(Flags ?? 0);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}