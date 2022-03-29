using Moonpie.Protocol.Network;
using Moonpie.Protocol.Packets.s2c.Play;
using Moonpie.Protocol.Protocol;

namespace Moonpie.Entities;

public class Player
{
    public Moonpie Proxy { get; init; }
    private readonly TransportManager _transportManager;
    public ProtocolVersion Version => _transportManager.Version;
    
    
    public event EventHandler? Disconnected
    {
        add => _transportManager.PlayerDisconnected += value;

        remove => _transportManager.PlayerDisconnected -= value;
    }
    
    public TransportManager Transport => _transportManager;
    
    
    public string Username { get; internal set; }
    internal Player(Moonpie proxy, PlayerConnection connection, string username)
    {
        this.Proxy = proxy;
        _transportManager = new TransportManager(this, connection);
        this.Username = username;
    }

    public async Task Connect(string host, uint port)
    {
        await _transportManager.Connect(host, port);
    }

    public async Task SendMessageAsync(ChatComponent message)
    {
        await _transportManager.PlayerTransport.Connection.WritePacketAsync(new ChatMessageS2CP()
        {
            Message = message,
            Position = ChatMessageS2CP.ChatTextPositions.SystemMessage
        });
    }
    
    public async Task SendMessageLinesAsync(params ChatComponent[] lines)
    {
        if (lines.Length == 0)
            return;
        
        if (lines.Length == 1)
        {
            await SendMessageAsync(lines[0]);
            return;
        }
                
        for (var i = 1; i < lines.Length; i++)
        {
            var line = lines[i];
            line.Text = "\n" + line.Text;
            lines[0].Add(line);
        }
        
        await SendMessageAsync(lines[0]);
    }
    
}