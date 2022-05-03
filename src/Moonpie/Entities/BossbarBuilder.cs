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
using Moonpie.Protocol.Protocol;

namespace Moonpie.Entities;

public class BossbarBuilder
{
    public float Health { get; set; }
    public BossbarColor Color { get; set; }
    public BossbarDivision Division { get; set; }
    public bool ShouldDarkenSky { get; set; }
    public bool IsDragonBar { get; set; }
    public bool CreateFog { get; set; }
    public ChatComponent Title { get; set; } = ChatComponent.Empty;
    
    public BossbarBuilder WithHealth(float health)
    {
        Health = health;
        return this;
    }
    
    public BossbarBuilder WithColor(BossbarColor color)
    {
        Color = color;
        return this;
    }
    
    public BossbarBuilder WithDivision(BossbarDivision division)
    {
        Division = division;
        return this;
    }
    
    public BossbarBuilder WithTitle(ChatComponent title)
    {
        Title = title;
        return this;
    }
    
    public BossbarBuilder SetDarkenSky(bool darkenSky)
    {
        ShouldDarkenSky = darkenSky;
        return this;
    }
    
    public BossbarBuilder SetDragonBar(bool dragonBar)
    {
        IsDragonBar = dragonBar;
        return this;
    }
    
    public BossbarBuilder SetCreateFog(bool createFog)
    {
        CreateFog = createFog;
        return this;
    }
    
}