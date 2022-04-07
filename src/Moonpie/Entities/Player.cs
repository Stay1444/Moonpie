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

using Moonpie.Entities.Models;
using Moonpie.Managers;
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
    public BossbarManager BossbarManager { get; }
    public string Username { get; internal set; }
    internal Player(Moonpie proxy, PlayerConnection connection, string username)
    {
        this.Proxy = proxy;
        _transportManager = new TransportManager(this, connection);
        this.Username = username;
        this.BossbarManager = new BossbarManager(this);
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