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

namespace Moonpie.Protocol.Packets.s2c.Login;

[PacketType(PacketTypes.S2C.LOGIN_ENCRYPTION_REQUEST)]
public class EncryptionRequestS2CP : IS2CPacket
{
    public string ServerId { get; set; } = String.Empty;
    public byte[] PublicKey { get; set; } = Array.Empty<byte>();
    public byte[] VerifyToken { get; set; }  = Array.Empty<byte>();
    

    public void Read(InByteBuffer buffer)
    {
        ServerId = buffer.ReadString();
        int pkLength = buffer.ReadVarInt();
        PublicKey = buffer.ReadBytes(pkLength);
        int vtLength = buffer.ReadVarInt();
        VerifyToken = buffer.ReadBytes(vtLength);
    }

    public void Write(OutByteBuffer buffer)
    {
        buffer.WriteString(ServerId);
        buffer.WriteVarInt(PublicKey.Length);
        buffer.WriteBytes(PublicKey);
        buffer.WriteVarInt(VerifyToken.Length);
        buffer.WriteBytes(VerifyToken);
    }
}