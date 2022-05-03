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

namespace Moonpie.Protocol.Packets.c2s.Handshaking;

[PacketType(PacketTypes.C2S.HandshakingHandshake)]
public class HandshakeC2SP : IC2SPacket
{
    public ProtocolVersion ProtocolVersion { get; set; }
    public string ServerAddress { get; set; }
    public ushort ServerPort { get; set; }
    public ProtocolState NextState { get; set; }

    public HandshakeC2SP()
    {
        ProtocolVersion = ProtocolVersion.v1_7_1_pre;
        ServerAddress = "localhost";
        ServerPort = 25565;
        NextState = ProtocolState.Status;
    }
    public void Read(InByteBuffer buffer)
    {
        int versionId = buffer.ReadVarInt();
        ProtocolVersion = ProtocolVersion.FromValue(versionId);
        ServerAddress = buffer.ReadString();
        ServerPort = buffer.ReadUShort();
        NextState = (ProtocolState) buffer.ReadVarInt();
    }

    public void Write(OutByteBuffer buffer)
    {
        buffer.WriteVarInt(ProtocolVersion.Value);
        buffer.WriteString(ServerAddress);
        buffer.WriteUShort(ServerPort);
        buffer.WriteVarInt((int) NextState);
    }
}