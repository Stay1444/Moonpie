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