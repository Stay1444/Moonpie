using System.Threading.Tasks;
using Moonpie.Protocol.Network;
using Moonpie.Protocol.Protocol;

namespace Moonpie.Protocol.Packets.c2s.Play;

[PacketType(PacketTypes.C2S.PLAY_CHAT_MESSAGE)]
public class ChatMessageC2SP : IC2SPacket
{
    public string Message { get; set; } = "";
    

    public void Read(InByteBuffer buffer)
    {
        Message = buffer.ReadString();
    }

    public void Write(OutByteBuffer buffer)
    {
        buffer.WriteString(Message);
    }

    public async Task Handle(PacketHandleContext handler)
    {
        if (Message.StartsWith("/"))
        {
            var found = await handler.Proxy.Plugins.TriggerCommandAsync(handler.Player, Message);
            if (found)
            {
                handler.Cancel();
            }
        }
    }
}