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

using System.Text.Json;
using Moonpie.Entities;
using Moonpie.Entities.Enums;
using Moonpie.Entities.Models;
using Moonpie.Entities.Models.Events;
using Moonpie.Protocol.Network;
using Moonpie.Protocol.Protocol;
using Moonpie.Utils.Protocol;
namespace Moonpie.Protocol.Packets.s2c.Play;

public enum BossbarAction
{
    Add,
    Remove,
    UpdateHealth,
    UpdateTitle,
    UpdateStyle,
    UpdateFlags
}


[PacketType(PacketTypes.S2C.PLAY_BOSS_BAR)]
public class BossbarS2CP : IS2CPacket
{

    
    public JavaUUID Uuid { get; set; }
    public BossbarAction Action { get; set; }
    public ChatComponent? Title { get; set; }
    private string? _translate;
    public float? Health { get; set; }
    public BossbarColor? Color { get; set; }
    public BossbarDivision? Division { get; set; }
    public byte? Flags { get; set; }

    public BossbarS2CP()
    {
        
    }

    public BossbarS2CP(BossbarAction action, Bossbar bossbar)
    {
        this.Uuid = bossbar.Id;
        this.Action = action;
        this.Title = bossbar.Title;
        this.Health = bossbar.Health;
        this.Color = bossbar.Color;
        this.Division = bossbar.Division;
        this.Flags = 0;
        
        if (bossbar.ShouldDarkenSky)
        {
            this.Flags.Value.SetBitMask(0x1);
        }

        if (bossbar.IsDragonBar)
        {
            this.Flags.Value.SetBitMask(0x2);
        }
        
        if (bossbar.CreateFog)
        {
            this.Flags.Value.SetBitMask(0x4);
        }
    }
    
    public void Read(InByteBuffer buffer)
    {
        Uuid = buffer.ReadUUID();
        Action = (BossbarAction)buffer.ReadVarInt();
        
        switch (Action)
        {
            case BossbarAction.Add:
                var json = buffer.ReadString();
                if (json.Contains("translate"))
                {
                    _translate = json;
                }else
                {
                    Title = JsonSerializer.Deserialize<ChatComponent>(json);
                }
                Health = buffer.ReadFloat();
                Color = (BossbarColor)buffer.ReadVarInt();
                Division = (BossbarDivision)buffer.ReadVarInt();
                Flags = buffer.ReadByte();
                break;
            case BossbarAction.Remove:
                // nothing
                break;
            case BossbarAction.UpdateHealth:
                Health = buffer.ReadFloat();
                break;
            case BossbarAction.UpdateTitle:
                var json2 = buffer.ReadString();
                if (json2.Contains("translate"))
                {
                    _translate = json2;
                }else
                {
                    Title = JsonSerializer.Deserialize<ChatComponent>(json2);
                }
                break;
            case BossbarAction.UpdateStyle:
                Color = (BossbarColor)buffer.ReadVarInt();
                Division = (BossbarDivision)buffer.ReadVarInt();
                break;
            case BossbarAction.UpdateFlags:
                Flags = buffer.ReadByte();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

    }

    public void Write(OutByteBuffer buffer)
    {
        buffer.WriteUUID(Uuid);
        buffer.WriteVarInt((int)Action);
        
        switch (Action)
        {
            case BossbarAction.Add:
                if (_translate != null)
                {
                    buffer.WriteString(_translate);
                }
                else
                {
                    buffer.WriteChatComponent(Title ?? ChatComponent.Empty);
                }

                buffer.WriteFloat(Health ?? 0);
                buffer.WriteVarInt((int)(Color ?? 0));
                buffer.WriteVarInt((int)(Division ?? 0));
                buffer.WriteByte(Flags ?? 0);
                break;
            case BossbarAction.Remove:
                // nothing
                break;
            case BossbarAction.UpdateHealth:
                buffer.WriteFloat(Health ?? 0);
                break;
            case BossbarAction.UpdateTitle:
                if (_translate != null)
                {
                    buffer.WriteString(_translate);
                }
                else
                {
                    buffer.WriteChatComponent(Title ?? ChatComponent.Empty);
                }
                break;
            case BossbarAction.UpdateStyle:
                buffer.WriteVarInt((int)(Color ?? 0));
                buffer.WriteVarInt((int)(Division ?? 0));
                break;
            case BossbarAction.UpdateFlags:
                buffer.WriteByte(Flags ?? 0);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public async Task Handle(PacketHandleContext handler)
    {
        handler.Cancel();
        handler.ExitEarly();

        if (Action == BossbarAction.Add)
        {
            var bossbarData = new BossbarData(this);
            var result =
                await handler.Proxy.Plugins.TriggerEventAsync(new BossbarAddEventArgs(handler.Proxy, handler.Player,
                    bossbarData));
            
            if (result is null) return;
            
            if (result.Canceled)
            {
                return;
            }
            
            await handler.Player.BossbarManager.i_CreateBossbar(bossbarData);

            this.Health = result.Bossbar.Health;
            this.Title = result.Bossbar.Title;
            this.Color = result.Bossbar.Color;
            this.Division = result.Bossbar.Division;
            this.Flags = 0;
            
            if (result.Bossbar.ShouldDarkenSky)
            {
                this.Flags.Value.SetBitMask(0x1);
            }

            if (result.Bossbar.IsDragonBar)
            {
                this.Flags.Value.SetBitMask(0x2);
            }
            
            if (result.Bossbar.CreateFog)
            {
                this.Flags.Value.SetBitMask(0x4);
            }
                    
            await handler.Player.Transport.PlayerTransport.Connection.WritePacketAsync(this);
            
            return;
        }
        
        if (Action == BossbarAction.Remove)
        {
            await handler.Proxy.Plugins.TriggerEventAsync(new BossbarRemoveEventArgs(this.Uuid, handler.Player, handler.Proxy));
            await handler.Player.BossbarManager.i_DeleteBossbar(this);
            await handler.Player.Transport.PlayerTransport.Connection.WritePacketAsync(this);
            return;
        }

        if (Action == BossbarAction.UpdateHealth)
        {
            if (this.Health is null) return;
            var bossbar = handler.Player.BossbarManager.GetBossbar(Uuid);
            if (bossbar == null)
            {
                return;
            }
            
            var bossbarData = new BossbarData(this);
            
            var result =
                await handler.Proxy.Plugins.TriggerEventAsync(new BossbarUpdateEventArgs(handler.Proxy, handler.Player,
                    Action, bossbar.GetDataClone(), bossbarData));
            
            if (result is null) return;

            if (result.New == bossbarData)
            {
                return;
            }

            this.Health = result.New.Health;
            await handler.Player.Transport.PlayerTransport.Connection.WritePacketAsync(this);
            
            return;
        }
        
        if (Action == BossbarAction.UpdateTitle)
        {
            if (this.Title is null) return;
            var bossbar = handler.Player.BossbarManager.GetBossbar(Uuid);
            if (bossbar == null)
            {
                return;
            }
            
            var bossbarData = new BossbarData(this);
            
            var result =
                await handler.Proxy.Plugins.TriggerEventAsync(new BossbarUpdateEventArgs(handler.Proxy, handler.Player,
                    Action, bossbar.GetDataClone(), bossbarData));
            
            if (result is null) return;

            if (result.New == bossbarData)
            {
                return;
            }

            this.Title = result.New.Title;
            await handler.Player.Transport.PlayerTransport.Connection.WritePacketAsync(this);
            
            return;
        }
        
        if (Action == BossbarAction.UpdateStyle)
        {
            if (this.Color is null) return;
            var bossbar = handler.Player.BossbarManager.GetBossbar(Uuid);
            if (bossbar == null)
            {
                return;
            }
            
            var bossbarData = new BossbarData(this);
            
            var result =
                await handler.Proxy.Plugins.TriggerEventAsync(new BossbarUpdateEventArgs(handler.Proxy, handler.Player,
                    Action, bossbar.GetDataClone(), bossbarData));
            
            if (result is null) return;

            if (result.New == bossbarData)
            {
                return;
            }

            this.Color = result.New.Color;
            this.Division = result.New.Division;
            await handler.Player.Transport.PlayerTransport.Connection.WritePacketAsync(this);
            
            return;
        }
        
        if (Action == BossbarAction.UpdateFlags)
        {
            if (this.Flags is null) return;
            var bossbar = handler.Player.BossbarManager.GetBossbar(Uuid);
            if (bossbar == null)
            {
                return;
            }
            
            var bossbarData = new BossbarData(this);
            
            var result =
                await handler.Proxy.Plugins.TriggerEventAsync(new BossbarUpdateEventArgs(handler.Proxy, handler.Player,
                    Action, bossbar.GetDataClone(), bossbarData));
            
            if (result is null) return;

            if (result.New == bossbarData)
            {
                return;
            }

            this.Flags = 0;
            if (result.New.ShouldDarkenSky)
            {
                this.Flags.Value.SetBitMask(0x1);
            }
            if (result.New.IsDragonBar)
            {
                this.Flags.Value.SetBitMask(0x2);
            }
            if (result.New.CreateFog)
            {
                this.Flags.Value.SetBitMask(0x4);
            }
            await handler.Player.Transport.PlayerTransport.Connection.WritePacketAsync(this);
            
            return;
        }
        
        throw new ArgumentOutOfRangeException();
    }
}