﻿using System.Security.Cryptography;

namespace Moonpie.Utils.Protocol;

public static class GuidUtils
{
    public static string NameUUIDFromBytes(byte[] input)
    {
        MD5 md5 = MD5.Create();
        byte[] hash = md5.ComputeHash(input);
        hash[6] &= 0x0f;
        hash[6] |= 0x30;
        hash[8] &= 0x3f;
        hash[8] |= 0x80;
        string hex = BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
        return hex.Insert(8, "-").Insert(13, "-").Insert(18, "-").Insert(23, "-");
    }
    
    public static unsafe void GuidToInt64(Guid value, out long x, out long y)
    {
        long* ptr = (long*)&value;
        x = *ptr++;
        y = *ptr;
    }
    
    public static unsafe Guid GuidFromInt64(long x, long y)
    {
        long* ptr = stackalloc long[2];
        ptr[0] = x;
        ptr[1] = y;
        return *(Guid*)ptr;
    }
    
    public static Guid GuidFromLongs(long a, long b)
    {
        byte[] guidData = new byte[16];
        Array.Copy(BitConverter.GetBytes(a), guidData, 8);
        Array.Copy(BitConverter.GetBytes(b), 0, guidData, 8, 8);
        return new Guid(guidData);
    }

    public static (long, long) ToLongs(this Guid guid)
    {
        var bytes = guid.ToByteArray();
        var long1 = BitConverter.ToInt64(bytes, 0);
        var long2 = BitConverter.ToInt64(bytes, 8);
        return (long1, long2);
    }
    
    [CLSCompliant(true)]
    public static Guid ToLittleEndian(this Guid javaGuid) {
        byte[] net = new byte[16];
        byte[] java = javaGuid.ToByteArray();
        for (int i = 8; i < 16; i++) {
            net[i] = java[i];
        }
        net[3] = java[0];
        net[2] = java[1];
        net[1] = java[2];
        net[0] = java[3];
        net[5] = java[4];
        net[4] = java[5];
        net[6] = java[7];
        net[7] = java[6];
        return new Guid(net);
    }

    /// <summary>
    /// Converts little-endian .NET guids to big-endian Java guids:
    /// </summary>
    [CLSCompliant(true)]
    public static Guid ToBigEndian(this Guid netGuid) {
        byte[] java = new byte[16];
        byte[] net = netGuid.ToByteArray();
        for (int i = 8; i < 16; i++) {
            java[i] = net[i];
        }
        java[0] = net[3];
        java[1] = net[2];
        java[2] = net[1];
        java[3] = net[0];
        java[4] = net[5];
        java[5] = net[4];
        java[6] = net[7];
        java[7] = net[6];
        return new Guid(java);
    }

}