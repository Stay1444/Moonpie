using System.Diagnostics;
using Moonpie.Entities;
using Moonpie.Protocol.Packets;
using Serilog;

namespace Moonpie.Protocol.Network;

public class PacketHandler
{
    public PacketHandler(Moonpie proxy, Player player, TransportManager transport)
    {
        Proxy = proxy;
        Player = player;
        Transport = transport;
    }

    public Moonpie Proxy { get; private set; }
    public Player Player { get; private set; }
    public TransportManager Transport { get; private set; }
    
    public async Task<bool> Handle(IPacket packet)
    {
        try
        {
            var ctx = new PacketHandleContext(Proxy, Player, Transport, this);
            var stopwatch = Stopwatch.StartNew();
            var earlyReturn = ctx.TExitEarly.Task;
            var handleTask = packet.Handle(ctx);
            await Task.WhenAny(earlyReturn, handleTask);
            stopwatch.Stop();
            if (stopwatch.ElapsedMilliseconds > 100)
            {
                Log.Warning("Packet {0} took {1}ms to handle", packet.GetType().Name, stopwatch.ElapsedMilliseconds);
            }
            return ctx.IsCanceled;
        }catch(Exception e)
        {
            Console.WriteLine(e);
        }

        return false;
    }
}