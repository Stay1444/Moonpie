using Moonpie.Entities;

namespace Moonpie.Protocol.Network;

public class PacketHandleContext
{
    public PacketHandleContext(Moonpie proxy, Player player, TransportManager transport, PacketHandler packetHandler)
    {
        Proxy = proxy;
        Player = player;
        Transport = transport;
        PacketHandler = packetHandler;
    }

    public Moonpie Proxy { get; init; }
    public Player Player { get; init; }
    public TransportManager Transport { get; init; }
    public PacketHandler PacketHandler { get; init; }
    public bool IsCanceled { get; private set; }
    public void Cancel()
    {
        IsCanceled = true;
    }

    internal TaskCompletionSource TExitEarly { get; set; } = new TaskCompletionSource(); 
    public void ExitEarly()
    {
        TExitEarly.SetResult();
    }
}