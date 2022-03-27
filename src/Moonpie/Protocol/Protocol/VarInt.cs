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
        FromBytes(bytes, out var size, out var value, offset);
        return new VarInt(value);
    }
}