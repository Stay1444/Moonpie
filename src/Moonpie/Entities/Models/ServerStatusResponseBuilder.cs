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

using System.Drawing;
using Moonpie.Protocol.Protocol;

namespace Moonpie.Entities.Models;

public class ServerStatusResponseBuilder
{
    public ChatComponent Description { get; private set; } = ChatComponent.Empty;
    public int MaxSlots { get; private set; }
    public int UsedSlots { get; private set; }
    
    public ProtocolVersion Version { get; private set; }
    
    public ServerStatusResponseBuilder WithDescription(ChatComponent chatComponent)
    {
        this.Description = chatComponent;
        return this;
    }
    
    public ServerStatusResponseBuilder WithPlayers(int usedSlots, int maxSlots)
    {
        this.MaxSlots = maxSlots;
        this.UsedSlots = usedSlots;
        return this;
    }
    
    public ServerStatusResponseBuilder WithVersion(ProtocolVersion version)
    {
        this.Version = version;
        return this;
    }
    
    
    
}