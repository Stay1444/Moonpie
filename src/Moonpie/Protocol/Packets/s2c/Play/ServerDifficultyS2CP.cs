using Moonpie.Protocol.Network;
using Moonpie.Protocol.Protocol;
using Moonpie.Utils.Protocol;

namespace Moonpie.Protocol.Packets.s2c.Play;

[PacketType(PacketTypes.S2C.PlayServerDifficulty)]
public class ServerDifficultyS2CP : IS2CPacket
{
    public enum Difficulties
    {
        Peaceful = 0,
        Easy = 1,
        Normal = 2,
        Hard = 3
    }
    
    public Difficulties Difficulty { get; set; }
    public bool? Locked { get; set; }
    
    public void Read(InByteBuffer buffer)
    {
        Difficulty = (Difficulties)buffer.ReadByte();
        Locked = buffer.ReadOptional(buffer.ReadBoolean);
    }

    public void Write(OutByteBuffer buffer)
    {
        buffer.WriteByte((byte)Difficulty);
        if (Locked is not null)
        {
            buffer.WriteBoolean(Locked.Value);
        }
    }
}