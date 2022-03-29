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

using Moonpie.Protocol.Network;
using Moonpie.Protocol.Protocol;

namespace Moonpie.Protocol.Packets.c2s.Play;

[PacketType(PacketTypes.C2S.PLAY_AUTOCOMPLETIONS)]
public class TabCompleteC2SP : IC2SPacket
{
    public int TransactionId { get; set; }
    public string Text { get; set; } = "";
    public void Read(InByteBuffer buffer)
    {
        TransactionId = buffer.ReadVarInt();
        Text = buffer.ReadString();
    }

    public void Write(OutByteBuffer buffer)
    {
        buffer.WriteVarInt(TransactionId);
        buffer.WriteString(Text);
    }

    public async Task Handle(PacketHandleContext handler)
    {
        if (await handler.Proxy.Plugins.HandleAutoCompletionAsync(handler.Player, TransactionId, Text))
        {
            handler.Cancel();
        }
    }
}