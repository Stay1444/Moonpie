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

namespace Moonpie.Protocol.Protocol;

public struct VarInt
{
    public int Value { get; }
    public int Size { get; }

    public VarInt(int value)
    {
        Value = value;

        Size = GetSize(value);
    }


    private static byte[] GetBytes(int v)
    {
        var bytes = new List<byte>();
        while (v > 0x7F)
        {
            bytes.Add((byte) (v | 0x80));
            v >>= 7;
        }

        bytes.Add((byte) v);
        return bytes.ToArray();
    }

    private static int GetSize(int v)
    {
        var size = 0;
        while (v > 0x7F)
        {
            size++;
            v >>= 7;
        }

        size++;
        return size;
    }

    private static void FromBytes(byte[] bytes, out int size, out int value, int offset)
    {
        value = 0;
        size = 0;
        for (var i = offset; i < bytes.Length; i++)
        {
            if (i > 1000) throw new Exception("VarInt is too big");

            value |= (bytes[i] & 0x7F) << (7 * i);
            size++;
            if ((bytes[i] & 0x80) == 0)
                break;
        }
    }

    public byte[] ToBytes()
    {
        return GetBytes(Value);
    }

    public VarInt(byte[] bytes, int offset = 0)
    {
        FromBytes(bytes, out var size, out var value, offset);
        Value = value;
        Size = size;
    }

    public static implicit operator VarInt(int value)
    {
        return new VarInt(value);
    }

    public static implicit operator int(VarInt value)
    {
        return value.Value;
    }

    public static VarInt Read(byte[] bytes, int offset = 0)
    {
        FromBytes(bytes, out _, out var value, offset);
        return new VarInt(value);
    }
}