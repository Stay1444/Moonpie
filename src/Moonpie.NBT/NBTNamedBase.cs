using System.Text;
using Moonpie.NBT.Utils;

namespace Moonpie.NBT;

public abstract class NBTNamedBase : NBTBase
{
    public string? Name { get; protected set; }
    protected NBTNamedBase(TagType type) : base(type)
    {
        
    }

    protected int ReadName(Span<byte> data, int index)
    {
        var nameLength = data.Slice(index, sizeof(short)).AsShortBE();
        index += sizeof(short);
        this.Name = data.Slice(index, nameLength).AsString();
        index += nameLength;
        return index;
    }

    protected void WriteName(Stream stream)
    {
        var nameBytes = Encoding.UTF8.GetBytes(this.Name ?? "");
        stream.WriteShortBE((short)nameBytes.Length);
        stream.Write(nameBytes);
    }
}