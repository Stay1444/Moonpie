namespace Moonpie.Utils.Protocol;

public static class ProtocolExtensions
{
    public static bool IsBitMask(this int value, int mask)
    {
        return (value & mask) == mask;
    }
    
    public static bool IsBitMask(this byte value, byte mask)
    {
        return (value & mask) == mask;
    }
    
    public static int SetBitMask(this int value, int mask)
    {
        return value | mask;
    }
    
    public static byte SetBitMask(this byte value, byte mask)
    {
        return (byte)(value | mask);
    }
    
}