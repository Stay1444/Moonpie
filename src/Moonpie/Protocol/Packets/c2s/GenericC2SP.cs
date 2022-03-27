using Moonpie.Protocol.Network;
using Moonpie.Protocol.Protocol;

namespace Moonpie.Protocol.Packets.c2s;

public class GenericC2SP : IC2SPacket
{
    public PacketTypes.C2S Type { get; }
    private byte[] _data = Array.Empty<byte>();

    public GenericC2SP(PacketTypes.C2S type)
    {
        Type = type;
    }

    public void Log()
    {
        Console.WriteLine("GenericC2SP");
    }
    
    public void Read(InByteBuffer buffer)
    {
        _data = buffer.ReadBytes(buffer.Length - buffer.Position);
    }

    public void Write(OutByteBuffer buffer)
    {
        buffer.WriteBytes(_data);
    }
}