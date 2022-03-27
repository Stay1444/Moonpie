using Moonpie.Protocol.Network;
using Moonpie.Protocol.Protocol;

namespace Moonpie.Protocol.Packets.c2s.Login;

[PacketType(PacketTypes.C2S.LOGIN_PLUGIN_RESPONSE)]
public class LoginPluginResponseC2SP : IC2SPacket
{
    public VarInt MessageId { get; set; }
    public bool Successful { get; set; }
    public byte[]? Data { get; set; }
    
    public void Read(InByteBuffer buffer)
    {
        MessageId = buffer.ReadVarInt();
        Successful = buffer.ReadBoolean();
        if (buffer.Position < buffer.Length)
        {
            Data = buffer.ReadBytes(buffer.Length - buffer.Position);
        }
    }

    public void Write(OutByteBuffer buffer)
    {
        buffer.WriteVarInt(MessageId);
        buffer.WriteBoolean(Successful);
        if (Data != null)
        {
            buffer.WriteBytes(Data);
        }
    }
}