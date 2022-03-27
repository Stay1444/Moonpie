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
    
    public async Task Handle(PacketHandleContext context)
    {
        await context.Transport.PlayerConnection.WritePacketAsync(new CompressionSetS2CP()
        {
            Threshold = 128
        });
        context.Transport.PlayerConnection.CompressionThreshold = 128;
        context.Transport.ServerConnection!.CompressionThreshold = Threshold;
        context.Cancel();
        Console.WriteLine($"Compression threshold set to {Threshold}");
    }
}