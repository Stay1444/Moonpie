using System.Text;
using System.Threading.Tasks;
using Moonpie.Protocol.Network;
using Moonpie.Protocol.Protocol;
using Moonpie.Utils.Protocol;

namespace Moonpie.Protocol.Packets.s2c.Play;

public class TabListDataS2CP : IS2CPacket
{

    public string Name { get; set; } = "";
    public int Ping { get; set; } = 0;
    public byte Action { get; set; } = 0;
    public JavaUUID UUID { get; set; } = JavaUUID.Empty;
    public void Read(InByteBuffer buffer)
    {
        
    }

    public void Write(OutByteBuffer buffer)
    {
        
    }

    public Task Handle(PacketHandleContext handler)
    {
        handler.Cancel();
        return Task.CompletedTask;
    }
}