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
}