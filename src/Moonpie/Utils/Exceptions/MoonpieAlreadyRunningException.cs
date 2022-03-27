namespace Moonpie.Utils.Exceptions;

public class MoonpieAlreadyRunningException : Exception
{
    public Moonpie Proxy { get; }
    public override string Message { get; }

    public MoonpieAlreadyRunningException(Moonpie proxy)
    {
        Proxy = proxy;
        Message = $"Moonpie is already running on {proxy.Host}:{proxy.Port}";
    }
}