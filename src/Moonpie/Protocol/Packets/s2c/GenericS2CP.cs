using Moonpie.Protocol.Network;
using Moonpie.Protocol.Protocol;

namespace Moonpie.Protocol.Packets.s2c;

public class GenericS2CP : IS2CPacket
{
    public PacketTypes.S2C Type { get; }
    public byte[] Data = Array.Empty<byte>();
    public GenericS2CP(PacketTypes.S2C type)
    {
        Type = type;
    }

    public void Log()
    {
        Console.WriteLine("GenericS2CP");
    }

    public void Read(InByteBuffer buffer)
    {
        Data = buffer.ReadBytes(buffer.Length - buffer.Position);
    }

    public void Write(OutByteBuffer buffer)
    {
        buffer.WriteBytes(Data);
    }

    public Task Handle(PacketHandleContext handler)
    {
        return Task.CompletedTask;
    }
}