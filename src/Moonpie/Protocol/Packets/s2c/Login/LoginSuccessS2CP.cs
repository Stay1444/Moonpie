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
        /*
        await handler.Transport.PlayerConnection.WritePacketAsync(new CompressionSetS2CP()
        {
            Threshold = 256
        });
        handler.Transport.PlayerConnection.CompressionThreshold = 256;
        */
        handler.Transport.ServerConnection!.State = ProtocolState.Play;
        handler.Transport.PlayerConnection.State = ProtocolState.Play;
        Log.Information("Login success");
    }
}