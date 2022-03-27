using Moonpie.Protocol.Network;
using Moonpie.Protocol.Protocol;

namespace Moonpie.Protocol.Packets.c2s.Login;
[PacketType(PacketTypes.C2S.LOGIN_ENCRYPTION_RESPONSE)]
public class EncryptionResponseC2SP : IC2SPacket
{   
    public int SharedSecretLength { get; set; }
    public byte[] SharedSecret { get; set; }
    public int VerifyTokenLength { get; set; }
    public byte[] VerifyToken { get; set; }
    
    public EncryptionResponseC2SP()
    {
        SharedSecretLength = 0;
        SharedSecret = new byte[0];
        VerifyTokenLength = 0;
        VerifyToken = new byte[0];
    }

    public void Read(InByteBuffer buffer)
    {
        SharedSecretLength = buffer.ReadVarInt();
        SharedSecret = buffer.ReadBytes(SharedSecretLength);
        VerifyTokenLength = buffer.ReadVarInt();
        VerifyToken = buffer.ReadBytes(VerifyTokenLength);
    }

    public void Write(OutByteBuffer buffer)
    {
        buffer.WriteVarInt(SharedSecretLength);
        buffer.WriteBytes(SharedSecret);
        buffer.WriteVarInt(VerifyTokenLength);
        buffer.WriteBytes(VerifyToken);
    }
}