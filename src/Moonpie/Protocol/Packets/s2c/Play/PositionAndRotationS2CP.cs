using Moonpie.Protocol.Network;
using Moonpie.Protocol.Protocol;
using Moonpie.Utils.Math;
using Moonpie.Utils.Protocol;

namespace Moonpie.Protocol.Packets.s2c.Play;

[PacketType(PacketTypes.S2C.PlayPositionAndRotation)]
public class PositionAndRotationS2CP : IS2CPacket
{
    public Vector3d Position { get; set; }
    public float Yaw { get; set; }
    public float Pitch { get; set; }
    public byte Flags { get; set; }
    public int TeleportId { get; set; }
    public bool Dismount { get; set; }
    
    public void Read(InByteBuffer buffer)
    {
        Position = buffer.ReadVector3d();
        Yaw = buffer.ReadFloat();
        Pitch = buffer.ReadFloat();
        Flags = buffer.ReadByte();
        TeleportId = buffer.ReadVarInt();
        Dismount = buffer.ReadBoolean();
    }

    public void Write(OutByteBuffer buffer)
    {
        buffer.WriteVector3d(Position);
        buffer.WriteFloat(Yaw);
        buffer.WriteFloat(Pitch);
        buffer.WriteByte(Flags);
        buffer.WriteVarInt(TeleportId);
        buffer.WriteBoolean(Dismount);
    }
}