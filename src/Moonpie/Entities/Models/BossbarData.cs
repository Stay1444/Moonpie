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

using Moonpie.Entities.Enums;
using Moonpie.Protocol.Packets.s2c.Play;
using Moonpie.Protocol.Protocol;
using Moonpie.Utils.Math;
using Moonpie.Utils.Protocol;

#pragma warning disable CS0649

namespace Moonpie.Entities.Models;

public class BossbarData
{

    public readonly JavaUUID Id;
    public ChatComponent? Title { get; set; }
    public float Health { get; set; }
    public BossbarColor Color { get; set; }
    public BossbarDivision Division { get; set; }
    public bool ShouldDarkenSky { get; set; }
    public bool IsDragonBar { get; set; }
    public bool CreateFog { get; set; }
    
    internal BossbarData(JavaUUID id)
    {
        this.Id = id;
    }

    internal BossbarData(BossbarS2CP packet)
    {
        this.Id = packet.Uuid;
        this.Title = packet.Title;
        this.Health = packet.Health ?? 0;
        this.Color = packet.Color ?? BossbarColor.Blue;
        this.Division = packet.Division ?? BossbarDivision.NoDivision;
        this.ShouldDarkenSky = packet.Flags.HasValue && packet.Flags.Value.IsBitMask(0x1);
        this.IsDragonBar = packet.Flags.HasValue && packet.Flags.Value.IsBitMask(0x2);
        this.CreateFog = packet.Flags.HasValue && packet.Flags.Value.IsBitMask(0x4);
    }
    
    internal BossbarData Clone()
    {
        return new BossbarData(Id)
        {
            Title = Title,
            Health = Health,
            Color = Color,
            Division = Division,
            ShouldDarkenSky = ShouldDarkenSky,
            IsDragonBar = IsDragonBar,
            CreateFog = CreateFog
        };
    }

    public override bool Equals(object? obj)
    {
        if (obj is BossbarData data)
        {
            return Id == data.Id &&
                   Title == data.Title &&
                   Health.IsEqual(data.Health) &&
                   Color == data.Color &&
                   Division == data.Division &&
                   ShouldDarkenSky == data.ShouldDarkenSky &&
                   IsDragonBar == data.IsDragonBar &&
                   CreateFog == data.CreateFog;
        }
        
        return false;
    }
    
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
    
    public static bool operator ==(BossbarData left, BossbarData right)
    {
        return left.Equals(right);
    }
    
    public static bool operator !=(BossbarData left, BossbarData right)
    {
        return !(left == right);
    }
}