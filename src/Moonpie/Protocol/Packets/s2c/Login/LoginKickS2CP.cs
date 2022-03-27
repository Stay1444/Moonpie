using System.Text.Json;
using Moonpie.Protocol.Network;
using Moonpie.Protocol.Protocol;
using Moonpie.Utils.Protocol;

namespace Moonpie.Protocol.Packets.s2c.Login;

[PacketType(PacketTypes.S2C.LOGIN_KICK)]
public class LoginKickS2CP : IS2CPacket
{
    public ChatComponent Reason { get; set; } = ChatComponent.Empty;
    

    public void Read(InByteBuffer buffer)
    {
        string json = buffer.ReadString();
        Reason = JsonSerializer.Deserialize<ChatComponent>(json)!;
    }

    public void Write(OutByteBuffer buffer)
    {
        buffer.WriteChatComponent(Reason);
    }
}