using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using Moonpie.Protocol.Protocol;

namespace Moonpie.Protocol.Network;

public class OutByteBuffer
{
    private static bool _isLittleEndian = BitConverter.IsLittleEndian;
    public static bool IsLittleEndian => _isLittleEndian;
    
    private List<byte> _bytes;
    public ProtocolVersion Version { get; }
    public ProtocolState State { get; }

    public int Length
    {
        get { return _bytes.Count; }
    }

    public byte[] Data => _bytes.ToArray();

    public OutByteBuffer(byte[] bytes, ProtocolVersion version, ProtocolState state)
    {
        _bytes = bytes.ToList();
        Version = version;
        State = state;
    }

    public OutByteBuffer(ProtocolVersion version, ProtocolState state)
    {
        _bytes = new List<byte>();
        Version = version;
        State = state;
    }

    public OutByteBuffer(OutByteBuffer buffer)
    {
        _bytes = buffer._bytes;
        Version = buffer.Version;
        State = buffer.State;
    }
    public void WriteBoolean(bool v)
    {
        _bytes.Add(v ? (byte) 1 : (byte) 0);
    }
    public void WriteShort(short v)
    {
        if (_isLittleEndian)
        {
            byte[] bytes = BitConverter.GetBytes(v);
            // reverse
            _bytes.AddRange(bytes.Reverse());
        }
        else
        {
            byte[] bytes = BitConverter.GetBytes(v);
            _bytes.AddRange(bytes);
        }
    }
    public void WriteUShort(ushort v)
    {
        if (_isLittleEndian)
        {
            byte[] bytes = BitConverter.GetBytes(v);
            // reverse
            _bytes.AddRange(bytes.Reverse());
        }
        else
        {
            byte[] bytes = BitConverter.GetBytes(v);
            _bytes.AddRange(bytes);
        }
    }
    public void WriteInt(int v)
    {
        if (_isLittleEndian)
        {
            byte[] bytes = BitConverter.GetBytes(v);
            // reverse
            _bytes.AddRange(bytes.Reverse());
        }
        else
        {
            byte[] bytes = BitConverter.GetBytes(v);
            _bytes.AddRange(bytes);
        }
    }
    public void WriteLong(long v)
    {
        if (_isLittleEndian)
        {
            byte[] bytes = BitConverter.GetBytes(v);
            // reverse
            _bytes.AddRange(bytes.Reverse());
        }
        else
        {
            byte[] bytes = BitConverter.GetBytes(v);
            _bytes.AddRange(bytes);
        }
    }
    public void WriteVarInt(int v)
    {
        var varint = new VarInt(v);
        _bytes.AddRange(varint.ToBytes());
    }
    public void WriteString(string v)
    {
        WriteVarInt(v.Length);
        _bytes.AddRange(Encoding.UTF8.GetBytes(v));
    }
    public void WriteBytes(byte[] v)
    {
        _bytes.AddRange(v);
    }
    public void WriteByte(byte v)
    {
        _bytes.Add(v);
    }
    public void WriteDouble(double v)
    {
        if (_isLittleEndian)
        {
            byte[] bytes = BitConverter.GetBytes(v);
            // reverse
            _bytes.AddRange(bytes.Reverse());
        }
        else
        {
            byte[] bytes = BitConverter.GetBytes(v);
            _bytes.AddRange(bytes);
        }
    }
    public void WriteFloat(float v)
    {
        if (_isLittleEndian)
        {
            byte[] bytes = BitConverter.GetBytes(v);
            // reverse
            _bytes.AddRange(bytes.Reverse());
        }
        else
        {
            byte[] bytes = BitConverter.GetBytes(v);
            _bytes.AddRange(bytes);
        }
    }
}