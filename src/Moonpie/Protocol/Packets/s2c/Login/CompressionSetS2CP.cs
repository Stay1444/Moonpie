using Moonpie.Protocol.Network;
using Moonpie.Protocol.Packets.c2s;
using Moonpie.Protocol.Protocol;

namespace Moonpie.Protocol.Packets.s2c.Login;

[PacketType(PacketTypes.S2C.LOGIN_COMPRESSION_SET)]
public class CompressionSetS2CP : IS2CPacket
{
    public int Threshold { get; set; }

    public void Read(InByteBuffer buffer)
    {
        Threshold = buffer.ReadVarInt();
    }

    public void Write(OutByteBuffer buffer)
    {
        buffer.WriteVarInt(Threshold);
    }
    
    public Task Handle(PacketHandleContext context)
    {
        context.Cancel();
        context.Transport.ServerTransport!.Connection.CompressionThreshold = Threshold;
        return Task.CompletedTask;
    }
}