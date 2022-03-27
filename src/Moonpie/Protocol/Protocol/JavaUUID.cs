﻿using System.Globalization;
using System.Security.Cryptography;

namespace Moonpie.Protocol.Protocol;
public readonly struct JavaUUID : IComparable<JavaUUID>, IEquatable<JavaUUID>
{
    private static RandomNumberGenerator? _generator;
    // The most significant 64 bits of this UUID.
    private readonly long mostSigBits;
    // The least significant 64 bits of this UUID.
    private readonly long leastSigBits;

    public long MostSigBits => mostSigBits;
    public long LeastSigBits => leastSigBits;
    
    public static JavaUUID Empty => new JavaUUID(0, 0);
    
    public JavaUUID(byte[] data)
    {
        long msb = 0;
        long lsb = 0;
        if (data.Length != 16)
        {
            throw new ArgumentException("The data must be 16 bytes in length");
        }

        for (int i = 0; i < 8; i++)
        {
            msb = (msb << 8) | (uint) (data[i] & 0xFF);
        }
        
        for (int i = 8; i < 16; i++)
        {
            lsb = (lsb << 8) | (uint) (data[i] & 0xFF);
        }
        
        this.mostSigBits = msb;
        this.leastSigBits = lsb;
    }
    public JavaUUID(long mostSigBits, long leastSigBits)
    {
        this.mostSigBits = mostSigBits;
        this.leastSigBits = leastSigBits;
    }

    public static JavaUUID Random()
    {
        if (_generator == null)
        {
            _generator = RandomNumberGenerator.Create();
        }
        
        byte[] randomBytes = new byte[16];
        _generator.GetBytes(randomBytes);
        randomBytes[6] &= 0x0F;  /* clear version        */
        randomBytes[6] |= 0x40;  /* set to version 4     */
        randomBytes[8] &= 0x3F;  /* clear variant        */
        randomBytes[8] |= 0x80;  /* set to IETF variant  */
        return new JavaUUID(randomBytes);
    }

    public JavaUUID(ulong bits)
    {
        this.mostSigBits = (long) (bits >> 32);
        this.leastSigBits = (long) (bits & 0xFFFFFFFFL);
    }

    public static JavaUUID NameUUIDFromBytes(byte[] name)
    {
        var md = MD5.Create();
        var hash = md.ComputeHash(name);
        hash[6] &= 0x0F;  /* clear version        */
        hash[6] |= 0x30;  /* set to version 3     */
        hash[8] &= 0x3F;  /* clear variant        */
        hash[8] |= 0x80;  /* set to IETF variant  */
        return new JavaUUID(hash);
    }

    public static JavaUUID FromString(string name)
    {
        string[] components = name.Split('-');
        if (components.Length != 5)
        {
            throw new ArgumentException("Invalid UUID string: " + name);
        }

        for (int i = 0; i < 5; i++)
        {
            components[i] = "0x" + components[i];
        }
        
        long mostSigBits = long.Parse(components[0], NumberStyles.HexNumber);
        mostSigBits <<= 16;
        mostSigBits |= long.Parse(components[1], NumberStyles.HexNumber);
        mostSigBits <<= 16;
        mostSigBits |= long.Parse(components[2], NumberStyles.HexNumber);
        
        long leastSigBits = long.Parse(components[3], NumberStyles.HexNumber);
        leastSigBits <<= 48;
        leastSigBits |= long.Parse(components[4], NumberStyles.HexNumber);
        
        return new JavaUUID(mostSigBits, leastSigBits);
    }

    public override string ToString()
    {
        return (Digits(mostSigBits >> 32, 8) + "-" +
                Digits(mostSigBits >> 16, 4) + "-" +
                Digits(mostSigBits, 4) + "-" +
                Digits(leastSigBits >> 48, 4) + "-" +
                Digits(leastSigBits, 12));
    }

    private static string Digits(long val, int digits)
    {
        long h1 = 1L << (digits * 4);
        long l2 = h1 | (val & (h1 - 1));
        // return l2 as hex string
        return l2.ToString("x").Substring(1);
    }

    public int CompareTo(JavaUUID other)
    {
        if (this.mostSigBits == other.mostSigBits)
        {
            return this.leastSigBits.CompareTo(other.leastSigBits);
        }
        else
        {
            return this.mostSigBits.CompareTo(other.mostSigBits);
        }
    }

    public bool Equals(JavaUUID other)
    {
        return (this.mostSigBits == other.mostSigBits) && (this.leastSigBits == other.leastSigBits);
    }

    public override bool Equals(object? o)
    {
        if (o == null)
        {
            return false;
        }
        
        if (o is JavaUUID uuid)
        {
            return this.Equals(uuid);
        }
        
        return false;
    }
    
    public bool Equals(JavaUUID? o)
    {
        if (o == null)
        {
            return false;
        }
        
        return this.mostSigBits == o.Value.mostSigBits && this.leastSigBits == o.Value.leastSigBits;
    }
    
    public override int GetHashCode()
    {
        return (int) (this.mostSigBits ^ this.leastSigBits);
    }
    
}

