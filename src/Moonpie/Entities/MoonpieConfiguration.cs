#region License
// Moonpie
// 
// Copyright (c) 2022 Stay
// 
// MIT License
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
#endregion

// ReSharper disable InconsistentNaming
namespace Moonpie.Entities;

public class MoonpieConfiguration
{
    public class MoonpieConfiguration_Motd
    {
        public string Motd { get; set; } = "Default Moonpie Motd";
        public int MaxPlayers { get; set; } = 20;
        public string Version { get; set; } = "Moonpie";
    }
    
    public class MoonpieConfiguration_Net
    {
        public int ServerConnectTimeout { get; set; } = 5000;
        public int CompressionThreshold { get; set; } = 256;
        public bool PingPassthrough { get; set; } = false;
        public string Host { get; set; } = "127.0.0.1";
        public int Port { get; set; } = 30000;
        public int MaxConnections { get; set; } = 0;
    }

    public class MoonpieConfiguration_Logs
    {
        public bool Pings { get; set; } = true;
        public bool Commands { get; set; } = true;
        public bool Connections { get; set; } = true;
        public bool LogToFile { get; set; } = true;
    }

    public class MoonpieConfiguration_Compatibility
    {
        public bool ForgeSupport { get; set; } = false;
        public bool IpForwarding { get; set; } = false;
    }

    public class MoonpieConfiguration_Security
    {
        public string OnlineMode { get; set; } = "enabled";
        public bool PreventProxyConnections { get; set; } = false;
        public int ConnectionThrottle { get; set; } = 5000;
        public int ConnectionThrottleLimit { get; set; } = 5;
        public int MaxConnectionsPerIp { get; set; } = 0;
    }

    public class MoonpieConfiguration_Others
    {
        public string[] DisabledCommands { get; set; } = new string[] { "example" };
    }

    public class MoonpieConfiguration_DefaultServer
    {
        public bool Enabled { get; set; } = false;
        public string Host { get; set; } = "127.0.0.1:25565";
    }

    public MoonpieConfiguration_Net Net { get; set; } = new MoonpieConfiguration_Net();
    public MoonpieConfiguration_Logs Logs { get; set; } = new MoonpieConfiguration_Logs();
    public MoonpieConfiguration_Compatibility Compatibility { get; set; } = new MoonpieConfiguration_Compatibility();
    public MoonpieConfiguration_Security Security { get; set; } = new MoonpieConfiguration_Security();
    public MoonpieConfiguration_DefaultServer DefaultServer { get; set; } = new MoonpieConfiguration_DefaultServer();
    public MoonpieConfiguration_Motd Motd { get; set; } = new MoonpieConfiguration_Motd();
    public MoonpieConfiguration_Others Others { get; set; } = new MoonpieConfiguration_Others();

    internal void Validate()
    {
        if (Security.OnlineMode != "enabled" && Security.OnlineMode != "disabled" && Security.OnlineMode != "hybrid")
            throw new Exception("Invalid online mode: " + Security.OnlineMode + ". Valid values are: enabled, disabled and hybrid.");
        
        if (Net.Port < 0 || Net.Port > 65535)
            throw new Exception("Invalid port: " + Net.Port + ". Valid values are between 0 and 65535.");
    }
}