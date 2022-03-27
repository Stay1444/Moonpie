using Moonpie.Protocol.Network;
using Moonpie.Protocol.Protocol;

namespace Moonpie.Protocol.Packets.s2c.Login;

[PacketType(PacketTypes.S2C.LOGIN_PLUGIN_REQUEST)]
public class LoginPluginRequestS2CP : IS2CPacket
{
    public int MessageId { get; set; } = -1;
    public string Channel { get; set; } = String.Empty;
    public byte[] Data { get; set; } = Array.Empty<byte>();
    

    public void Read(InByteBuffer buffer)
    {
        MessageId = buffer.ReadVarInt();
        Channel = buffer.ReadString();
        Data = buffer.ReadBytes(buffer.Length - buffer.Position);
    }

    public void Write(OutByteBuffer buffer)
    {
        buffer.WriteVarInt(MessageId);
        buffer.WriteString(Channel);
        buffer.WriteBytes(Data);
    }
}