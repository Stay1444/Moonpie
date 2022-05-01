using System.IO.Compression;
using Moonpie.NBT.Exceptions;
using Moonpie.NBT.Tags;

namespace Moonpie.NBT;

public static class NBTSerializer
{
    private static byte[] ReadFully(Stream input)
    {
        using MemoryStream ms = new MemoryStream();
        while (true)
        {
            byte[] buffer = new byte[1024];
            int read = input.Read(buffer, 0, buffer.Length);
            if (read <= 0)
                break;
            ms.Write(buffer, 0, read);
        }
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

        }else if (type == TagType.TagList)
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
        using var stream = new MemoryStream();

        if (tag is INBTSerializable serializable)
        {
            stream.WriteByte((byte)tag.Type.Value);
            serializable.Serialize(stream);
        }
        else
        {
            throw new UnsupportedTagTypeException(tag.Type);
        }

        if (compression == NBTCompression.GZip)
        {
            var output = new MemoryStream();
            using var gzip = new GZipStream(output, CompressionMode.Compress);
            stream.WriteTo(gzip);
            gzip.Flush();
            gzip.Close();
            
            return output.ToArray();
        }else
        {
            return stream.ToArray();
        }
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