using System.Text;

namespace Moonpie.NBT.Utils;

internal static class SpanExtensions
{
    public static short AsShortBE(this Span<byte> span)
    {
        span.Reverse();
        return BitConverter.ToInt16(span);
    }

    public static short AsShortBE(this Span<byte> span, int index)
    {
        var bytes = span.Slice(index, 2);
        bytes.Reverse();
        return BitConverter.ToInt16(bytes);
    }

    public static short AsShortLE(this Span<byte> span)
    {
        return BitConverter.ToInt16(span);
    }
    
    public static short AsShortLE(this Span<byte> span, int index)
    {
        return BitConverter.ToInt16(span.Slice(index, 2));
    }
    
    public static int AsIntBE(this Span<byte> span)
    {
        span.Reverse();
        return BitConverter.ToInt32(span);
    }
    
    public static int AsIntBE(this Span<byte> span, int index)
    {
        var bytes = span.Slice(index, 4);
        bytes.Reverse();
        return BitConverter.ToInt32(bytes);
    }
    
    public static int AsIntLE(this Span<byte> span)
    {
        return BitConverter.ToInt32(span);
    }
    
    public static int AsIntLE(this Span<byte> span, int index)
    {
        return BitConverter.ToInt32(span.Slice(index, 4));
    }
    
    public static long AsLongBE(this Span<byte> span)
    {
        span.Reverse();
        return BitConverter.ToInt64(span);
    }
    
    public static long AsLongBE(this Span<byte> span, int index)
    {
        var bytes = span.Slice(index, 8);
        bytes.Reverse();
        return BitConverter.ToInt64(bytes);
    }
    
    public static long AsLongLE(this Span<byte> span)
    {
        return BitConverter.ToInt64(span);
    }
    
    public static long AsLongLE(this Span<byte> span, int index)
    {
        return BitConverter.ToInt64(span.Slice(index, 8));
    }
    
    public static float AsFloatBE(this Span<byte> span)
    {
        span.Reverse();
        return BitConverter.ToSingle(span);
    }
    
    public static float AsFloatBE(this Span<byte> span, int index)
    {
        var bytes = span.Slice(index, 4);
        bytes.Reverse();
        return BitConverter.ToSingle(bytes);
    }
    
    public static float AsFloatLE(this Span<byte> span)
    {
        return BitConverter.ToSingle(span);
    }
    
    public static float AsFloatLE(this Span<byte> span, int index)
    {
        return BitConverter.ToSingle(span.Slice(index, 4));
    }
    
    public static double AsDoubleBE(this Span<byte> span)
    {
        span.Reverse();
        return BitConverter.ToDouble(span);
    }
    
    public static double AsDoubleBE(this Span<byte> span, int index)
    {
        var bytes = span.Slice(index, 8);
        bytes.Reverse();
        return BitConverter.ToDouble(bytes);
    }
    
    public static double AsDoubleLE(this Span<byte> span)
    {
        return BitConverter.ToDouble(span);
    }
    
    public static double AsDoubleLE(this Span<byte> span, int index)
    {
        return BitConverter.ToDouble(span.Slice(index, 8));
    }
    
    public static ushort AsUShortBE(this Span<byte> span)
    {
        span.Reverse();
        return BitConverter.ToUInt16(span);
    }
    
    public static ushort AsUShortBE(this Span<byte> span, int index)
    {
        var bytes = span.Slice(index, 2);
        bytes.Reverse();
        return BitConverter.ToUInt16(bytes);
    }
    public static ushort AsUShortLE(this Span<byte> span)
    {
        return BitConverter.ToUInt16(span);
    }
    
    public static ushort AsUShortLE(this Span<byte> span, int index)
    {
        return BitConverter.ToUInt16(span.Slice(index, 2));
    }
    
    public static uint AsUIntBE(this Span<byte> span)
    {
        span.Reverse();
        return BitConverter.ToUInt32(span);
    }
    
    public static uint AsUIntBE(this Span<byte> span, int index)
    {
        var bytes = span.Slice(index, 4);
        bytes.Reverse();
        return BitConverter.ToUInt32(bytes);
    }
    
    public static uint AsUIntLE(this Span<byte> span)
    {
        return BitConverter.ToUInt32(span);
    }
    
    public static uint AsUIntLE(this Span<byte> span, int index)
    {
        return BitConverter.ToUInt32(span.Slice(index, 4));
    }
    
    public static ulong AsULongBE(this Span<byte> span)
    {
        span.Reverse();
        return BitConverter.ToUInt64(span);
    }
    
    public static ulong AsULongBE(this Span<byte> span, int index)
    {
        var bytes = span.Slice(index, 8);
        bytes.Reverse();
        return BitConverter.ToUInt64(bytes);
    }
    
    public static ulong AsULongLE(this Span<byte> span)
    {
        return BitConverter.ToUInt64(span);
    }
    
    public static ulong AsULongLE(this Span<byte> span, int index)
    {
        return BitConverter.ToUInt64(span.Slice(index, 8));
    }
    public static string AsString(this Span<byte> span)
    {
        return Encoding.UTF8.GetString(span);
    }
    
    public static string AsString(this Span<byte> span, int index, int length)
    {
        return Encoding.UTF8.GetString(span.Slice(index, length));
    }
}