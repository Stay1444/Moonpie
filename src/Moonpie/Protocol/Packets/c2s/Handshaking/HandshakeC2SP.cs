using Moonpie.Protocol.Network;
using Moonpie.Protocol.Protocol;

namespace Moonpie.Protocol.Packets.c2s.Handshaking;

[PacketType(PacketTypes.C2S.HANDSHAKING_HANDSHAKE)]
public class HandshakeC2SP : IC2SPacket
{
    public ProtocolVersion ProtocolVersion { get; set; }
    public string ServerAddress { get; set; }
    public ushort ServerPort { get; set; }
    public ProtocolState NextState { get; set; }

    public HandshakeC2SP()
    {
        ProtocolVersion = ProtocolVersion.v1_7_1_pre;
        ServerAddress = "localhost";
        ServerPort = 25565;
        NextState = ProtocolState.Status;
    }
    public void Read(InByteBuffer buffer)
    {
        int versionId = buffer.ReadVarInt();
        ProtocolVersion = ProtocolVersion.FromValue(versionId);
        ServerAddress = buffer.ReadString();
        ServerPort = buffer.ReadUShort();
        NextState = (ProtocolState) buffer.ReadVarInt();
    }

    public void Write(OutByteBuffer buffer)
    {
        buffer.WriteVarInt(ProtocolVersion.Value);
        buffer.WriteString(ServerAddress);
        buffer.WriteUShort(ServerPort);
        buffer.WriteVarInt((int) NextState);
    }
}