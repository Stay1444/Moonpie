using System.IO.Compression;
using Moonpie.NBT.Exceptions;
using Moonpie.NBT.Tags;

namespace Moonpie.NBT;

public static class NBTSerializer
{
    private static byte[] ReadFully(Stream input)
    {
        using MemoryStream ms = new MemoryStream();
        input.CopyTo(ms);
        return ms.ToArray();
    }
    
    public static NBTBase Deserialize(byte[] data, NBTCompression compression = NBTCompression.None)
    {
        if (compression == NBTCompression.GZip)
        {
            var gzip = new GZipStream(new MemoryStream(data), CompressionMode.Decompress);
            data = ReadFully(gzip);
        }
        TagType type = TagType.FromValue(data[0]);

        if (type == TagType.Compound)
        {
            return ReadCompound(data);

        }else if (type == TagType.List)
        {
            return ReadList(data);
        }
        else
        {
            throw new UnsupportedTagTypeException(type);
        }
    }

    public static byte[] Serialize(NBTBase tag, NBTCompression compression = NBTCompression.None)
    {
        throw new NotImplementedException();
    }
    
    private static NBTCompound ReadCompound(Span<byte> data)
    {
        var compound = new NBTCompound();
        compound.Deserialize(data, 1);
        return compound;
    }

    private static NBTList ReadList(Span<byte> data)
    {
        var list = new NBTList();
        list.Deserialize(data, 1);
        return list;
    }
}