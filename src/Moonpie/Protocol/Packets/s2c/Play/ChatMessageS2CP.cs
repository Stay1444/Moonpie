using System.Text.Json;
using Moonpie.Protocol.Network;
using Moonpie.Protocol.Protocol;
using Moonpie.Utils.Protocol;

namespace Moonpie.Protocol.Packets.s2c.Play;

[PacketType(PacketTypes.S2C.PLAY_CHAT_MESSAGE)]
public class ChatMessageS2CP : IS2CPacket
{
    public ChatComponent? Message { get; set; } = ChatComponent.Empty;
    public string? Translate { get; set; }
    public ChatTextPositions Position { get; set; } = ChatTextPositions.SystemMessage;
    public JavaUUID? Sender { get; set; } = null;
    public void Read(InByteBuffer buffer)
    {
        string json = buffer.ReadString();
        if (json.Contains("translate"))
        {
            Translate = json;
        }else
        {
            Message = JsonSerializer.Deserialize<ChatComponent>(json);
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
            buffer.WriteString(JsonSerializer.Serialize(Message));
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