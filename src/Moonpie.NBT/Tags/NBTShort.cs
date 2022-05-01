using System.Text;
using Moonpie.NBT.Utils;

namespace Moonpie.NBT.Tags;

public class NBTShort : NBTNamedBase, INBTSerializable
{
    internal NBTShort() : base(TagType.Short)
    {
        
    }
    
    public NBTShort(string name, short value) : base(TagType.Short)
    {
        Value = value;
        Name = name;
    }

    public short Value { get; set; }
    
    public int Deserialize(Span<byte> data, int index, bool named = true)
    {
        if (named)
        {
            index = ReadName(data, index);
        }

        Value = data.AsShortBE(index);
        index += sizeof(short);
        return index;
    }

    public void Serialize(Stream stream, bool named = true)
    {
        if (named) WriteName(stream);
        stream.WriteShortBE(Value);
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append(TagType.Short.Name);
        if (this.Name is null)
        {
            sb.Append("(None): ");
        }else
        {
            sb.Append($"('{this.Name}'): ");
        }
        sb.Append(this.Value);
        
        return sb.ToString();
    }
}