using Moonpie.Protocol.Network;
using Moonpie.Protocol.Protocol;

namespace Moonpie.Protocol.Packets.c2s.Login;

[PacketType(PacketTypes.C2S.LOGIN_LOGIN_START)]
public class LoginStartC2SP : IC2SPacket
{
    public string? Username { get; set; }
    
    public void Read(InByteBuffer buffer)
    {
        Username = buffer.ReadString();
    }

    public void Write(OutByteBuffer buffer)
    {
        buffer.WriteString(Username ?? "");
    }
}