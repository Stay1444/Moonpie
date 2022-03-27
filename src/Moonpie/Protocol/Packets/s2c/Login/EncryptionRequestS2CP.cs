using Moonpie.Protocol.Network;
using Moonpie.Protocol.Protocol;

namespace Moonpie.Protocol.Packets.s2c.Login;

[PacketType(PacketTypes.S2C.LOGIN_ENCRYPTION_REQUEST)]
public class EncryptionRequestS2CP : IS2CPacket
{
    public string ServerId { get; set; } = String.Empty;
    public byte[] PublicKey { get; set; } = Array.Empty<byte>();
    public byte[] VerifyToken { get; set; }  = Array.Empty<byte>();
    

    public void Read(InByteBuffer buffer)
    {
        ServerId = buffer.ReadString();
        int pkLength = buffer.ReadVarInt();
        PublicKey = buffer.ReadBytes(pkLength);
        int vtLength = buffer.ReadVarInt();
        VerifyToken = buffer.ReadBytes(vtLength);
    }

    public void Write(OutByteBuffer buffer)
    {
        buffer.WriteString(ServerId);
        buffer.WriteVarInt(PublicKey.Length);
        buffer.WriteBytes(PublicKey);
        buffer.WriteVarInt(VerifyToken.Length);
        buffer.WriteBytes(VerifyToken);
    }
}