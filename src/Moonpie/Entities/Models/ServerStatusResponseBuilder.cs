using System.Drawing;
using Moonpie.Protocol.Protocol;

namespace Moonpie.Entities.Models;

public class ServerStatusResponseBuilder
{
    public ChatComponent Description { get; private set; } = ChatComponent.Empty;
    public int MaxSlots { get; private set; }
    public int UsedSlots { get; private set; }
    
    public ProtocolVersion Version { get; private set; }
    
    public ServerStatusResponseBuilder WithDescription(ChatComponent chatComponent)
    {
        this.Description = chatComponent;
        return this;
    }
    
    public ServerStatusResponseBuilder WithPlayers(int usedSlots, int maxSlots)
    {
        this.MaxSlots = maxSlots;
        this.UsedSlots = usedSlots;
        return this;
    }
    
    public ServerStatusResponseBuilder WithVersion(ProtocolVersion version)
    {
        this.Version = version;
        return this;
    }
    
    
    
}