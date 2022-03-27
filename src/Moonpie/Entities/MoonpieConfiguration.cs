namespace Moonpie.Entities;

public class MoonpieConfiguration
{
    public class MoonpieConfiguration_FallbackServer
    {
        public bool Enabled { get; set; } = false;
        public string Host { get; set; } = "127.0.0.1";
        public ushort Port { get; set; } = 25566;
    }
    
    public MoonpieConfiguration(string host, ushort port, uint maxPlayers)
    {
        Host = host;
        Port = port;
        MaxPlayers = maxPlayers;
    }

    public MoonpieConfiguration()
    {
        Host = "127.0.0.1";
        Port = 25565;
        MaxPlayers = 20;
    }

    public string Host { get; init; }
    public ushort Port { get; init; }
    public uint MaxPlayers { get; init; }
    
    public MoonpieConfiguration_FallbackServer Fallback { get; init; } = new MoonpieConfiguration_FallbackServer();   
}