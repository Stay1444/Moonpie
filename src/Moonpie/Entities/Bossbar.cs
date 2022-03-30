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

using Moonpie.Entities.Models;
using Moonpie.Protocol.Packets.s2c.Play;
using Moonpie.Protocol.Protocol;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Moonpie.Entities;

public class Bossbar
{
    private Player _player;
    public JavaUUID Uuid { get; internal set; }
    public ChatComponent Title { get; internal set; } = new ChatComponent("Unnamed");
    public int Health { get; internal set; }
    public BossbarColor Color { get; internal set; }
    public BossbarDivision Division { get; internal set; }
    internal Bossbar(Player player, JavaUUID uuid)
    {
        this.Uuid = uuid;
        this._player = player;
    }

    public async Task ModifyAsync(Action<BossbarModifyModel> action)
    {
        var bossbarEditModel = new BossbarModifyModel();
        action(bossbarEditModel);

        bossbarEditModel.Health = Math.Clamp(bossbarEditModel.Health ?? 0, 0, 100);
        
        if (bossbarEditModel.Health != null && bossbarEditModel.Health != Health)
        {
            var packet = new BossbarS2CP
            {
                Action = BossbarAction.UpdateHealth,
                Uuid = Uuid,
                Health = bossbarEditModel.Health / 100f
            };
            
            await _player.Transport.PlayerTransport.Connection.WritePacketAsync(packet);
        }
        
        if (bossbarEditModel.Division != null && bossbarEditModel.Color != null && bossbarEditModel.Color != Color && bossbarEditModel.Division != Division)
        {
            var packet = new BossbarS2CP
            {
                Action = BossbarAction.UpdateStyle,
                Uuid = Uuid,
                Color = bossbarEditModel.Color,
                Division = bossbarEditModel.Division
            };

            await _player.Transport.PlayerTransport.Connection.WritePacketAsync(packet);
        }
        else if (bossbarEditModel.Color != null && bossbarEditModel.Color != Color)
        {
            var packet = new BossbarS2CP
            {
                Action = BossbarAction.UpdateStyle,
                Uuid = Uuid,
                Color = bossbarEditModel.Color
            };
            
            await _player.Transport.PlayerTransport.Connection.WritePacketAsync(packet);
        }else if (bossbarEditModel.Division != null && bossbarEditModel.Division != Division)
        {
            var packet = new BossbarS2CP
            {
                Action = BossbarAction.UpdateStyle,
                Uuid = Uuid,
                Division = bossbarEditModel.Division
            };
            
            await _player.Transport.PlayerTransport.Connection.WritePacketAsync(packet);
        }
        
        if (bossbarEditModel.Title != null && bossbarEditModel.Title != Title)
        {
            var packet = new BossbarS2CP
            {
                Action = BossbarAction.UpdateTitle,
                Uuid = Uuid,
                Title = bossbarEditModel.Title
            };
            
            await _player.Transport.PlayerTransport.Connection.WritePacketAsync(packet);
        }
        
        this.Color = bossbarEditModel.Color ?? BossbarColor.Blue;
        this.Division = bossbarEditModel.Division ?? BossbarDivision.NoDivision;
        this.Health = bossbarEditModel.Health ?? 0;
    }

    internal void Set(BossbarModifyModel model)
    {
        this.Color = model.Color ?? BossbarColor.Blue;
        this.Division = model.Division ?? BossbarDivision.NoDivision;
        this.Health = model.Health ?? 0;
        this.Title = model.Title ?? new ChatComponent("Unnamed");
    }
}