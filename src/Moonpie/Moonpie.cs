using System.Net;
using System.Net.Sockets;
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
    public bool IsRunning { get; private set; }
    private List<Player> _players = new List<Player>();
    public IReadOnlyCollection<Player> Players => _players;
    public string Host { get; }
    public int Port { get; }
    public PluginManager PluginManager { get; }
    public Moonpie(MoonpieConfiguration configuration)
    {
        this._configuration = configuration;
        _tcpListener = new TcpListener(IPAddress.Parse(configuration.Host), configuration.Port);
        IsRunning = false;
        Host = configuration.Host;
        Port = configuration.Port;
        _connectionHandler = new ConnectionHandler(configuration, this);
        PluginManager = new PluginManager(this);
        PluginManager.Load();
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