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
// FITNESS FOR A PARTICULAR PURPOSE AND NON INFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
#endregion

using Moonpie.Protocol.Protocol;

namespace Moonpie.Entities;

public class TitleBuilder
{
    public ChatComponent Text { get; set; } = ChatComponent.Empty;
    public TimeSpan FadeIn { get; set; } = TimeSpan.FromSeconds(1);
    public TimeSpan FadeOut { get; set; } = TimeSpan.FromSeconds(1);
    public TimeSpan Duration { get; set; } = TimeSpan.FromSeconds(1);
    
    public ChatComponent? Subtitle { get; set; }
    
    public TitleBuilder WithText(ChatComponent component)
    {
        this.Text = component;
        return this;
    }
    
    public TitleBuilder WithText(string text, params object[] args)
    {
        this.Text = string.Format(text, args);
        return this;
    }
    
    public TitleBuilder WithFadeIn(TimeSpan fadeIn)
    {
        this.FadeIn = fadeIn;
        return this;
    }
    
    public TitleBuilder WithFadeOut(TimeSpan fadeOut)
    {
        this.FadeOut = fadeOut;
        return this;
    }
    
    public TitleBuilder WithDuration(TimeSpan duration)
    {
        this.Duration = duration;
        return this;
    }
    
    public TitleBuilder WithSubtitle(ChatComponent component)
    {
        this.Subtitle = component;
        return this;
    }
    
    public TitleBuilder RemoveSubtitle()
    {
        this.Subtitle = null;
        return this;
    }
}