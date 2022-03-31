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

using Moonpie.Entities;
using Moonpie.Entities.Enums;
using Moonpie.Entities.Models;
using Moonpie.Protocol.Packets.s2c.Play;
using Moonpie.Protocol.Protocol;
using Moonpie.Utils.Protocol;

namespace Moonpie.Managers;

public class BossbarManager
{
    public Player Player { get; }
    internal BossbarManager(Player player)
    {
        this.Player = player;
    }
    
    private readonly Dictionary<JavaUUID, Bossbar> _bossbars = new ();
    
    public IReadOnlyDictionary<JavaUUID, Bossbar> Bossbars => _bossbars;

    public async Task<Bossbar> CreateBossbar(ChatComponent title, BossbarColor color, BossbarDivision division, float health,
        bool shouldDarkenSky = false, bool isDragonBar = false, bool createFog = false)
    {
        var bossbarId = JavaUUID.Random();
        var bossbar = new Bossbar(this, BossbarOwner.Moonpie)
        {
            Title = title,
            Color = color,
            Division = division,
            Health = health,
            Id = bossbarId,
            ShouldDarkenSky = shouldDarkenSky,
            IsDragonBar = isDragonBar,
            CreateFog = createFog,
        };
        
        _bossbars.Add(bossbarId, bossbar);
        await Player.Transport.PlayerTransport.Connection.WritePacketAsync(new BossbarS2CP(BossbarAction.Add, bossbar));
        return bossbar;
    }

    internal Task<Bossbar> i_CreateBossbar(BossbarData data)
    {
        var bossbarId = data.Id;
        var bossbar = new Bossbar(this, BossbarOwner.Minecraft)
        {
            Title = data.Title ?? new ChatComponent(""),
            Color = data.Color,
            Division = data.Division,
            Health = data.Health,
            Id = bossbarId,
            ShouldDarkenSky = data.ShouldDarkenSky,
            IsDragonBar = data.IsDragonBar,
            CreateFog = data.CreateFog,
        };
        
        _bossbars.Add(bossbarId, bossbar);
        
        return Task.FromResult(bossbar);
    }
    
    internal Task i_DeleteBossbar(BossbarS2CP packet)
    {
        var bossbarId = packet.Uuid;
        if (_bossbars.TryGetValue(bossbarId, out var bossbar))
        {
            _bossbars.Remove(bossbarId);
        }
        
        return Task.CompletedTask;
    }
    
    public Task<Bossbar> CreateBossbar(BossbarBuilder builder)
    {
        return CreateBossbar(builder.Title, builder.Color, builder.Division, builder.Health, builder.ShouldDarkenSky, builder.IsDragonBar, builder.CreateFog);
    }

    public Bossbar? GetBossbar(JavaUUID id)
    {
        if (Bossbars.TryGetValue(id, out var bossbar))
        {
            return bossbar;
        }
        
        return null;
    }
    
    public async Task DeleteBossbar(JavaUUID bossbar)
    {
        if (!_bossbars.TryGetValue(bossbar, out var bossbar1))
        {
            return;
        }
        
        _bossbars.Remove(bossbar);
        await Player.Transport.PlayerTransport.Connection.WritePacketAsync(new BossbarS2CP(BossbarAction.Remove, bossbar1));
    }

    public Task DeleteBossbar(Bossbar bossbar)
    {
        return DeleteBossbar(bossbar.Id);
    }
    
    internal async Task SetBossbarTitle(Bossbar bossbar, ChatComponent title)
    {
        bossbar.Title = title;
        await Player.Transport.PlayerTransport.Connection.WritePacketAsync(new BossbarS2CP(BossbarAction.UpdateTitle, bossbar));
    }
    
    internal async Task SetBossbarHealth(Bossbar bossbar, float health)
    {
        bossbar.Health = health;
        await Player.Transport.PlayerTransport.Connection.WritePacketAsync(new BossbarS2CP(BossbarAction.UpdateHealth, bossbar));
    }
    
    internal async Task SetBossbarStyle(Bossbar bossbar, BossbarColor color, BossbarDivision division)
    {
        bossbar.Color = color;
        bossbar.Division = division;
        await Player.Transport.PlayerTransport.Connection.WritePacketAsync(new BossbarS2CP(BossbarAction.UpdateStyle, bossbar));
    }
    
}