using Moonpie.Protocol.Network;
using Moonpie.Protocol.Protocol;

namespace Moonpie.Protocol.Packets.s2c.Play;

public class DeclareCommandsS2CP : IS2CPacket
{
    public class CommandNode
    {
        public bool IsExecutable { get; set; }
    }
    
    public void Read(InByteBuffer buffer)
    {
        throw new NotImplementedException();
    }

    public void Write(OutByteBuffer buffer)
    {
        throw new NotImplementedException();
    }
}