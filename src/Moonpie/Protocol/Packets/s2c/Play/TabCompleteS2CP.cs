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
using Moonpie.Utils.Protocol;
namespace Moonpie.Protocol.Packets.s2c.Play;

[PacketType(PacketTypes.S2C.PLAY_AUTOCOMPLETIONS)]
public class TabCompleteS2CP : IS2CPacket
{
    public int TransactionId { get; set; }
    public int StartIndex { get; set; }
    public int Length { get; set; }

    public record Match
    {
        public string MatchText { get; set; } = "";
        public bool HasTooltip { get; set; } = false;
        public ChatComponent Tooltip { get; set; } = ChatComponent.Empty;
    }
    
    public List<Match> Matches { get; set; } = new List<Match>(); 
    
    public void Read(InByteBuffer buffer)
    {
        TransactionId = buffer.ReadVarInt();
        StartIndex = buffer.ReadVarInt();
        Length = buffer.ReadVarInt();
        Matches = buffer.ReadArray(buffer.ReadVarInt(), () =>
        {
            Match match = new Match();
            match.MatchText = buffer.ReadString();
            match.HasTooltip = buffer.ReadBoolean();
            if (match.HasTooltip)
            {
                match.Tooltip = buffer.ReadChatComponent();
            }
            return match;
        }).ToList();
    }

    public void Write(OutByteBuffer buffer)
    {
        buffer.WriteVarInt(TransactionId);
        buffer.WriteVarInt(StartIndex);
        buffer.WriteVarInt(Length);
        buffer.WriteVarInt(Matches.Count);
        buffer.WriteArray(Matches, match =>
        {
            buffer.WriteString(match.MatchText);
            buffer.WriteBoolean(match.HasTooltip);
            if (match.HasTooltip)
            {
                buffer.WriteChatComponent(match.Tooltip);
            }
        });
    }
}