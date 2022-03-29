using Moonpie.Protocol.Network;
using Moonpie.Protocol.Protocol;

namespace Moonpie.Protocol.Packets.c2s.Play;

[PacketType(PacketTypes.C2S.PLAY_AUTOCOMPLETIONS)]
public class TabCompleteC2SP : IC2SPacket
{
    public int TransactionId { get; set; }
    public string Text { get; set; } = "";
    public void Read(InByteBuffer buffer)
    {
        TransactionId = buffer.ReadVarInt();
        Text = buffer.ReadString();
    }

    public void Write(OutByteBuffer buffer)
    {
        buffer.WriteVarInt(TransactionId);
        buffer.WriteString(Text);
    }

    public async Task Handle(PacketHandleContext handler)
    {
        if (await handler.Proxy.Plugins.HandleAutoCompletionAsync(handler.Player, TransactionId, Text))
        {
            handler.Cancel();
        }
    }
}