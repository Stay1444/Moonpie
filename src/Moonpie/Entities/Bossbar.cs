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
using Moonpie.Entities.Models;
using Moonpie.Managers;
using Moonpie.Protocol.Protocol;
using Moonpie.Utils.Math;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Moonpie.Entities;

public class Bossbar
{
    /// <summary>
    /// The unique identifier of the bossbar.
    /// </summary>
    public JavaUUID Id
    {
        get => _data.Id;
    }

    /// <summary>
    /// The title of the bossbar.
    /// </summary>
    public ChatComponent Title
    {
        get => _data.Title ?? ChatComponent.Empty;
        internal set => _data.Title = value;
    }

    /// <summary>
    /// Health of the bossbar.
    /// 0.0f - 1.0f
    /// </summary>
    public float Health
    {
        get => _data.Health;
        internal set => _data.Health = value;
    }

    /// <summary>
    /// Color of the bossbar.
    /// <see cref="BossbarColor"/>
    /// </summary>
    public BossbarColor Color
    {
        get => _data.Color;
        internal set => _data.Color = value;
    }

    /// <summary>
    /// Divisions of the bossbar.
    /// <see cref="BossbarDivision"/>
    /// </summary>
    public BossbarDivision Division
    {
        get => _data.Division;
        internal set => _data.Division = value;
    }

    /// <summary>
    /// Should darken the bossbar.
    /// </summary>
    public bool ShouldDarkenSky
    {
        get => _data.ShouldDarkenSky;
        internal set => _data.ShouldDarkenSky = value;
    }

    /// <summary>
    /// Is dragon bar.
    /// </summary>
    public bool IsDragonBar
    {
        get => _data.IsDragonBar;
        internal set => _data.IsDragonBar = value;
    }

    /// <summary>
    /// Create fog.
    /// </summary>
    public bool CreateFog
    {
        get => _data.CreateFog;
        internal set => _data.CreateFog = value;
    }
    
    /// <summary>
    /// Owner of the bossbar.
    /// </summary>
    public BossbarOwner Owner { get; internal set; }
    
    private readonly BossbarManager _manager;
    private readonly BossbarData _data;
    internal Bossbar(BossbarManager manager, BossbarOwner owner, JavaUUID id)
    {
        _manager = manager;
        Owner = owner;
        _data = new BossbarData(id);
    }
    
    internal BossbarData GetDataClone()
    {
        return _data.Clone();
    }
    
    /// <summary>
    /// Deletes the bossbar.
    /// </summary>
    public Task DeleteAsync() => _manager.DeleteBossbar(this);

    public async Task ModifyAsync(Action<BossbarModifyModel> action)
    {
        var model = new BossbarModifyModel();
        action(model);
        if (model.Title != null && !model.Title.Equals(Title))
        {
            await _manager.SetBossbarTitle(this, model.Title);
        }
        
        if (model.Health != null && !model.Health.IsEqual(Health))
        {
            await _manager.SetBossbarHealth(this, model.Health.Value);
        }

        if (model.Color != null || model.Division != null)
        {
            if (model.Color != null && model.Division != null && model.Color != Color && model.Division != Division)
            {
                await _manager.SetBossbarStyle(this, model.Color.Value, model.Division.Value);
            }else if (model.Color != null && model.Color != Color)
            {
                await _manager.SetBossbarStyle(this, model.Color.Value, Division);
            }else if (model.Division != null && model.Division != Division)
            {
                await _manager.SetBossbarStyle(this, Color, model.Division.Value);
            }
        }
    }
}