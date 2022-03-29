using System.Threading.Tasks;
using Moonpie.Protocol.Network;
using Moonpie.Protocol.Protocol;
using Moonpie.Utils.Protocol;
using Serilog;

namespace Moonpie.Protocol.Packets.s2c.Login;

[PacketType(PacketTypes.S2C.LOGIN_LOGIN_SUCCESS)]
public class LoginSuccessS2CP : IS2CPacket
{
    public JavaUUID Uuid { get; set; }
    public string? Name { get; set; }
    
    public void Read(InByteBuffer buffer)
    {
        Uuid = buffer.Version < ProtocolVersion.v20w12a ? buffer.ReadUUIDString() : buffer.ReadUUID();
        Name = buffer.ReadString();
    }

    public void Write(OutByteBuffer buffer)
    {  
        if (buffer.Version < ProtocolVersion.v20w12a)
        {
            buffer.WriteUUIDString(Uuid);
        }
        else
        {
            buffer.WriteUUID(Uuid);
        }
        
        buffer.WriteString(Name ?? "");
    }

    public async Task Handle(PacketHandleContext handler)
    {
        
        await handler.Transport.PlayerTransport.Connection.WritePacketAsync(new CompressionSetS2CP()
        {
            Threshold = handler.Proxy.Configuration.CompressionThreshold
        });
        handler.Transport.PlayerTransport.Connection.CompressionThreshold = handler.Proxy.Configuration.CompressionThreshold;
        
        handler.Transport.ServerTransport!.Connection.State = ProtocolState.Play;
        handler.Transport.PlayerTransport.Connection.State = ProtocolState.Play;
    }
}