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

using System.Text;
using System.Text.Json;
using Moonpie.Protocol.Protocol;
using System.Linq;
namespace Moonpie.Protocol.Network;

public class InByteBuffer
{
    private static bool _isLittleEndian = BitConverter.IsLittleEndian;
    public static bool IsLittleEndian => _isLittleEndian;
    private byte[] _bytes;
    private int _position;
    public ProtocolVersion Version { get; }
    public ProtocolState State { get; }
    public int Length
    {
        get { return _bytes.Length; }
    }
    
    public void Reset()
    {
        _position = 0;
    }

    public int Position
    {
        get => _position;
        set => _position = value;
    } 
    
    public InByteBuffer(byte[] bytes, ProtocolVersion version, ProtocolState state)
    {
        _bytes = bytes.ToArray();
        this.Version = version;
        _position = 0;
        this.State = state;
    }
    
    public InByteBuffer(InByteBuffer buffer)
    {
        _bytes = buffer._bytes.ToArray();
        Version = buffer.Version;
        _position = buffer._position;
        State = buffer.State;
    }
    
    public bool ReadBoolean()
    {
        return _bytes[_position++] != 0;
    }
    public short ReadShort()
    {
        if (_isLittleEndian)
        {
            byte[] bytes = new byte[2];
            bytes[0] = _bytes[_position++];
            bytes[1] = _bytes[_position++];
            return BitConverter.ToInt16(bytes.Reverse().ToArray(), 0);
        }
        else
        {
            byte[] bytes = new byte[2];
            bytes[0] = _bytes[_position++];
            bytes[1] = _bytes[_position++];
            return BitConverter.ToInt16(bytes, 0);
        }
    }
    public ushort ReadUShort()
    {
        if (_isLittleEndian)
        {
            byte[] bytes = new byte[2];
            bytes[0] = _bytes[_position++];
            bytes[1] = _bytes[_position++];
            return BitConverter.ToUInt16(bytes.Reverse().ToArray(), 0);
        }
        else
        {
            byte[] bytes = new byte[2];
            bytes[0] = _bytes[_position++];
            bytes[1] = _bytes[_position++];
            return BitConverter.ToUInt16(bytes, 0);
        }
    }
    public int ReadInt()
    {
        if (_isLittleEndian)
        {
            byte[] bytes = new byte[4];
            Array.Copy(_bytes, _position, bytes, 0, 4);
            _position += 4;
            return BitConverter.ToInt32(bytes.Reverse().ToArray(), 0);
        }
        else
        {
            byte[] bytes = new byte[4];
            Array.Copy(_bytes, _position, bytes, 0, 4);
            _position += 4;
            return BitConverter.ToInt32(bytes, 0);
        }
    }
    public long ReadLong(bool skipEndianness = false)
    {
        if (_isLittleEndian && !skipEndianness)
        {
            byte[] bytes = new byte[8];
            Array.Copy(_bytes, _position, bytes, 0, 8);
            _position += 8;
            return BitConverter.ToInt64(bytes.Reverse().ToArray(), 0);
        }
        else
        {
            byte[] bytes = new byte[8];
            Array.Copy(_bytes, _position, bytes, 0, 8);
            _position += 8;
            return BitConverter.ToInt64(bytes, 0);
        }
    }

    public uint ReadUInt()
    {
        if (_isLittleEndian)
        {
            byte[] bytes = new byte[4];
            Array.Copy(_bytes, _position, bytes, 0, 4);
            _position += 4;
            return BitConverter.ToUInt32(bytes.Reverse().ToArray(), 0);
        }
        else
        {
            byte[] bytes = new byte[4];
            Array.Copy(_bytes, _position, bytes, 0, 4);
            _position += 4;
            return BitConverter.ToUInt32(bytes, 0);
        }
    }
    
    public int ReadVarInt()
    {
        int value = 0;
        int size = 0;
        byte b;
        do
        {
            b = _bytes[_position++];
            value |= (b & 0x7F) << size++ * 7;
            if (size > 5)
            {
                throw new OverflowException("VarInt is too big");
            }
        } while ((b & 0x80) == 0x80);
        return value;
    }
    public string ReadString()
    {
        int length = ReadVarInt();
        byte[] bytes = new byte[length];
        Array.Copy(_bytes, _position, bytes, 0, length);
        _position += length;
        return Encoding.UTF8.GetString(bytes);
    }
    public string ReadString(int length)
    {
        byte[] bytes = new byte[length];
        Array.Copy(_bytes, _position, bytes, 0, length);
        _position += length;
        return Encoding.UTF8.GetString(bytes);
    }
    public byte[] ReadBytes(int c)
    {
        byte[] bytes = new byte[c];
        Array.Copy(_bytes, _position, bytes, 0, c);
        _position += c;
        return bytes;
    }
    public float ReadFloat()
    {
        if (_isLittleEndian)
        {
            byte[] bytes = new byte[4];
            Array.Copy(_bytes, _position, bytes, 0, 4);
            _position += 4;
            return BitConverter.ToSingle(bytes.Reverse().ToArray(), 0);
        }
        else
        {
            byte[] bytes = new byte[4];
            Array.Copy(_bytes, _position, bytes, 0, 4);
            _position += 4;
            return BitConverter.ToSingle(bytes, 0);
        }
    }
    public double ReadDouble()
    {
        if (_isLittleEndian)
        {
            byte[] bytes = new byte[8];
            Array.Copy(_bytes, _position, bytes, 0, 8);
            _position += 8;
            return BitConverter.ToDouble(bytes.Reverse().ToArray(), 0);
        }
        else
        {
            byte[] bytes = new byte[8];
            Array.Copy(_bytes, _position, bytes, 0, 8);
            _position += 8;
            return BitConverter.ToDouble(bytes, 0);
        }
    }
    public byte ReadByte()
    {
        return _bytes[_position++];
    }
    public byte[] ReadRest()
    {
        byte[] bytes = new byte[_bytes.Length - _position];
        Array.Copy(_bytes, _position, bytes, 0, bytes.Length);
        _position = _bytes.Length;
        return bytes;
    }

    public ulong ReadULong()
    {
        if (_isLittleEndian)
        {
            byte[] bytes = new byte[8];
            Array.Copy(_bytes, _position, bytes, 0, 8);
            _position += 8;
            return BitConverter.ToUInt64(bytes.Reverse().ToArray(), 0);
        }
        else
        {
            byte[] bytes = new byte[8];
            Array.Copy(_bytes, _position, bytes, 0, 8);
            _position += 8;
            return BitConverter.ToUInt64(bytes, 0);
        }
    }
    
    
}