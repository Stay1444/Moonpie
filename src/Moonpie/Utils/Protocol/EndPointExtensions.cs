using System.Net;

namespace Moonpie.Utils.Protocol;

public static class EndPointExtensions
{
    public static int GetPort(this EndPoint endPoint)
    {
        if (endPoint is IPEndPoint)
        {
            return ((IPEndPoint)endPoint).Port;
        }
        else if (endPoint is DnsEndPoint)
        {
            return ((DnsEndPoint)endPoint).Port;
        }
        else
        {
            throw new NotSupportedException();
        }
    }
    
    public static string GetHost(this EndPoint endPoint)
    {
        if (endPoint is IPEndPoint)
        {
            return ((IPEndPoint)endPoint).Address.ToString();
        }
        else if (endPoint is DnsEndPoint)
        {
            return ((DnsEndPoint)endPoint).Host;
        }
        else
        {
            throw new NotSupportedException();
        }
    }
}