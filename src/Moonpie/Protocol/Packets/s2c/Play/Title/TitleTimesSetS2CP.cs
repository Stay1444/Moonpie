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

using Moonpie.Protocol.Network;
using Moonpie.Protocol.Protocol;

namespace Moonpie.Protocol.Packets.s2c.Play.Title;

[PacketType(PacketTypes.S2C.PLAY_TITLE_TIMES_SET)]
public class TitleTimesSetS2CP : IS2CPacket
{
    public int FadeInTime { get; set; }
    public int StayTime { get; set; }
    public int FadeOutTime { get; set; }
    
    public void Read(InByteBuffer buffer)
    {
        FadeInTime = buffer.ReadInt();
        StayTime = buffer.ReadInt();
        FadeOutTime = buffer.ReadInt();
    }

    public void Write(OutByteBuffer buffer)
    {
        buffer.WriteInt(FadeInTime);
        buffer.WriteInt(StayTime);
        buffer.WriteInt(FadeOutTime);
    }
}