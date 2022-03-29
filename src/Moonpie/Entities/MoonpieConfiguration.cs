#region Copyright
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
    public int CompressionThreshold { get; init; } = 256;
    public MoonpieConfiguration_FallbackServer Fallback { get; init; } = new MoonpieConfiguration_FallbackServer();   
}