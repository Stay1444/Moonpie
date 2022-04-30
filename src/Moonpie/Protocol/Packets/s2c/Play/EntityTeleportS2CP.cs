using Moonpie.Protocol.Network;
using Moonpie.Protocol.Protocol;
using Moonpie.Utils.Math;
using Moonpie.Utils.Protocol;

namespace Moonpie.Protocol.Packets.s2c.Play;

[PacketType(PacketTypes.S2C.PLAY_ENTITY_TELEPORT)]
public class EntityTeleportS2CP : IS2CPacket
{
    public int EntityId { get; set; }
    public Vector3d Position { get; set; }
    public byte Yaw { get; set; }
    public byte Pitch { get; set; }
    public bool OnGround { get; set; }

    public void Read(InByteBuffer buffer)
    {
        EntityId = buffer.ReadVarInt();
        Position = buffer.ReadVector3d();
        Yaw = buffer.ReadByte();
        Pitch = buffer.ReadByte();
        OnGround = buffer.ReadBoolean();
    }

    public void Write(OutByteBuffer buffer)
    {
        buffer.WriteVarInt(EntityId);
        buffer.WriteVector3d(Position);
        buffer.WriteByte(Yaw);
        buffer.WriteByte(Pitch);
        buffer.WriteBoolean(OnGround);
    }
}