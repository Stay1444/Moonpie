using System.Text;

namespace Moonpie.NBT.Utils;

internal static class StreamExtensions
{
    public static void WriteShortBE(this Stream stream, short value)
    {
        var bytes = BitConverter.GetBytes(value);
        stream.Write(bytes.Reverse().ToArray());
    }

    public static void WriteShortLE(this Stream stream, short value)
    {
        var bytes = BitConverter.GetBytes(value);
        stream.Write(bytes);
    }
    
    public static void WriteUShortBE(this Stream stream, ushort value)
    {
        var bytes = BitConverter.GetBytes(value);
        stream.Write(bytes.Reverse().ToArray());
    }
    
    public static void WriteUShortLE(this Stream stream, ushort value)
    {
        var bytes = BitConverter.GetBytes(value);
        stream.Write(bytes);
    }
    
    public static void WriteIntBE(this Stream stream, int value)
    {
        var bytes = BitConverter.GetBytes(value);
        stream.Write(bytes.Reverse().ToArray());
    }
    
    public static void WriteIntLE(this Stream stream, int value)
    {
        var bytes = BitConverter.GetBytes(value);
        stream.Write(bytes);
    }
    
    public static void WriteUIntBE(this Stream stream, uint value)
    {
        var bytes = BitConverter.GetBytes(value);
        stream.Write(bytes.Reverse().ToArray());
    }
    
    public static void WriteUIntLE(this Stream stream, uint value)
    {
        var bytes = BitConverter.GetBytes(value);
        stream.Write(bytes);
    }
    
    public static void WriteLongBE(this Stream stream, long value)
    {
        var bytes = BitConverter.GetBytes(value);
        stream.Write(bytes.Reverse().ToArray());
    }
    
    public static void WriteLongLE(this Stream stream, long value)
    {
        var bytes = BitConverter.GetBytes(value);
        stream.Write(bytes);
    }
    
    public static void WriteULongBE(this Stream stream, ulong value)
    {
        var bytes = BitConverter.GetBytes(value);
        stream.Write(bytes.Reverse().ToArray());
    }
    
    public static void WriteULongLE(this Stream stream, ulong value)
    {
        var bytes = BitConverter.GetBytes(value);
        stream.Write(bytes);
    }
    
    public static void WriteFloatBE(this Stream stream, float value)
    {
        var bytes = BitConverter.GetBytes(value);
        stream.Write(bytes.Reverse().ToArray());
    }
    
    public static void WriteFloatLE(this Stream stream, float value)
    {
        var bytes = BitConverter.GetBytes(value);
        stream.Write(bytes);
    }
    
    public static void WriteDoubleBE(this Stream stream, double value)
    {
        var bytes = BitConverter.GetBytes(value);
        stream.Write(bytes.Reverse().ToArray());
    }
    
    public static void WriteDoubleLE(this Stream stream, double value)
    {
        var bytes = BitConverter.GetBytes(value);
        stream.Write(bytes);
    }
}