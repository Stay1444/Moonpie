using System.Text;
using Moonpie.NBT.Utils;

namespace Moonpie.NBT.Tags;

public class NBTLong : NBTNamedBase, INBTSerializable
{
    internal NBTLong() : base(TagType.Long)
    {
        
    }
    
    public NBTLong(string name, long value) : base(TagType.Long)
    {
        Value = value;
        this.Name = name;
    }
    
    public long Value { get; set; }

    public int Deserialize(Span<byte> data, int index, bool named = true)
    {
        if (named)
        {
            index = ReadName(data, index);
        }
        
        Value = data.AsLongBE(index);
        index += sizeof(long);
        return index;
    }

    public void Serialize(Stream stream, bool named = true)
    {
        if (named) WriteName(stream);
        stream.WriteLongBE(Value);
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append(TagType.Long.Name);
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