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
using Moonpie.Protocol.Network;
using Moonpie.Protocol.Protocol;
using Moonpie.Utils.Math;

namespace Moonpie.Utils.Protocol;

public static class OutByteBufferExtensions
{
    public static void WriteChatComponent(this OutByteBuffer buffer, ChatComponent c)
    {
        string json = JsonSerializer.Serialize(c);
        buffer.WriteString(json);
    }
    
    public static void WriteJson(this OutByteBuffer buffer, object v)
    {
        string json = JsonSerializer.Serialize(v);
        buffer.WriteString(json);
    }
    
    // ReSharper disable InconsistentNaming
    public static void WriteUUIDString(this OutByteBuffer buffer, JavaUUID guid)
    {
        buffer.WriteString(guid.ToString());
    }
    
    public static void WriteUUID(this OutByteBuffer buffer, JavaUUID? guid)
    {
        if (guid is not null)
        {
            buffer.WriteLong(guid.Value.MostSigBits);
            buffer.WriteLong(guid.Value.LeastSigBits);
        }
        else
        {
            buffer.WriteLong(0);
            buffer.WriteLong(0);
        }
    }
    public static void WritePlayerProperties(this OutByteBuffer buffer, PlayerProperties properties)
    {
        buffer.WriteVarInt(0);
    }
    
    public static void WriteArray<T>(this OutByteBuffer buffer, IEnumerable<T> enumerable, Action<T> write)
    {
        foreach (var item in enumerable)
        {
            write(item);
        }
    }
    
    public static void WriteVector3d(this OutByteBuffer buffer, Vector3d vector)
    {
        buffer.WriteDouble(vector.X);
        buffer.WriteDouble(vector.Y);
        buffer.WriteDouble(vector.Z);
    }
    
}