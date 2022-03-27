using System.Net;
using Moonpie.Protocol.Protocol;

namespace Moonpie.Entities.Models.Events;

public class PlayerPingEventArgs : MoonpieEventArgs
{
    public EndPoint EndPoint { get; }
    public ProtocolVersion Version { get; }

    public ServerStatusResponseBuilder? Response { get; set; }
    public PlayerPingEventArgs(EndPoint endPoint, ProtocolVersion version)
    {
        EndPoint = endPoint;
        Version = version;
    }
}