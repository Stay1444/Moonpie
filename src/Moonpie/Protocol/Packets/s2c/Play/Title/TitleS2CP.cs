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
using Moonpie.Utils.Protocol;

namespace Moonpie.Protocol.Packets.s2c.Play.Title;

[PacketType(PacketTypes.S2C.PLAY_TITLE)]
public class TitleS2CP : IS2CPacket
{
    public enum TitleActions
    {
        TitleText,
        Subtitle,
        HotbarText,
        Times,
        Hide,
        Reset
    }

    public interface ITitleAction
    {
        public void Read(InByteBuffer buffer);
        public void Write(OutByteBuffer buffer);
    }

    public class TitleActionText : ITitleAction
    {
        public ChatComponent Text { get; set; } = ChatComponent.Empty;
        
        public void Read(InByteBuffer buffer)
        {
            Text = buffer.ReadChatComponent();
        }

        public void Write(OutByteBuffer buffer)
        {
            buffer.WriteChatComponent(Text);
        }
    }

    public class TitleActionTimes : ITitleAction
    {
        public int FadeInTime { get; set; } = 0;
        public int StayTime { get; set; } = 0;
        public int FadeOutTime { get; set; } = 0;
        public void Read(InByteBuffer buffer)
        {
            FadeInTime = buffer.ReadInt();
            StayTime = buffer.ReadInt();
            FadeOutTime = buffer.ReadInt();
        }

        public void Write(OutByteBuffer buffer)
        {
            buffer.WriteInt(FadeInTime);
            buffer.WriteInt(StayTime);
            buffer.WriteInt(FadeOutTime);
        }
    }
    
    public TitleActions Action { get; set; }
    public ITitleAction? ActionData { get; set; }
    
    public void Read(InByteBuffer buffer)
    {
        var action = buffer.ReadVarInt();
        Action = (TitleActions) action;

        ActionData = Action switch
        {
            TitleActions.TitleText => new TitleActionText(),
            TitleActions.Subtitle => new TitleActionText(),
            TitleActions.HotbarText => new TitleActionText(),
            TitleActions.Times => new TitleActionTimes(),
            _ => null
        };

        ActionData?.Read(buffer);
    }

    public void Write(OutByteBuffer buffer)
    {
        buffer.WriteVarInt((int) Action);
        ActionData?.Write(buffer);
    }
}