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

global using System.Net;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using Moonpie.Entities;
using Moonpie.Managers;
using Moonpie.Protocol.Network;
using Moonpie.Utils.Exceptions;
using Serilog;

namespace Moonpie;

public class Moonpie
{
    private MoonpieConfiguration _configuration;
    private readonly TcpListener _tcpListener;
    private readonly ConnectionHandler _connectionHandler;
    public MoonpieConfiguration Configuration => _configuration;
    public bool IsRunning { get; private set; }
    private List<Player> _players = new List<Player>();
    public IReadOnlyCollection<Player> Players => _players;
    public string Host { get; }
    public int Port { get; }
    public PluginManager Plugins { get; }
    public Moonpie(MoonpieConfiguration configuration)
    {
        this._configuration = configuration;
        _tcpListener = new TcpListener(IPAddress.Parse(configuration.Host), configuration.Port);
        IsRunning = false;
        Host = configuration.Host;
        Port = configuration.Port;
        _connectionHandler = new ConnectionHandler(configuration, this);
        Plugins = new PluginManager(this);
        Plugins.Load();
    }

    public void Start()
    {
        if (IsRunning)
        {
            throw new MoonpieAlreadyRunningException(this);
        }
        
        _tcpListener.Start();
        IsRunning = true;
        Log.Information("Moonpie started on {host}:{port}", Host, Port);
        Task.Run(AcceptClientsAsync);
    }

    public void Stop()
    {
        if (!IsRunning)
        {
            return;
        }        
        
        _tcpListener.Stop();
        IsRunning = false;
        Log.Information("Moonpie stopped on {host}:{port}", Host, Port);
    }

    private async Task AcceptClientsAsync()
    {
        while (IsRunning && _tcpListener.Server.IsBound)
        {
            var client = await _tcpListener.AcceptTcpClientAsync();
            _ = AcceptClientAsync(client);
        }
    }

    private async Task AcceptClientAsync(TcpClient client)
    {
        var player = await _connectionHandler.HandleConnectionAsync(client);
        if (player is null) return;
        Log.Information("Player {username} connected", player.Username);
        _players.Add(player);
        player.Disconnected += PlayerDisconnected;
    }

    private void PlayerDisconnected(object? sender, EventArgs e)
    {
        var player = (Player) sender!;
        Log.Information("Player {username} disconnected", player.Username);
        _players.Remove(player);
    }
}