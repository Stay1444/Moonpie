using System.Text;
using Moonpie.NBT.Utils;

namespace Moonpie.NBT.Tags;

public class NBTLongArray : NBTNamedBase, INBTSerializable
{
    internal NBTLongArray() : base(TagType.LongArray)
    {
        
    }
    
    public NBTLongArray(string name, long[] value) : base(TagType.LongArray)
    {
        Value = value;
        Name = name;
    }
    
    public long[] Value { get; set; } = Array.Empty<long>();
    public int Deserialize(Span<byte> data, int index, bool named = true)
    {
        if (named)
        {
            index = ReadName(data, index);
        }

        var length = data.AsIntBE(index);
        index += sizeof(int);
        
        Value = new long[length];
        
        for (int i = 0; i < length; i++)
        {
            Value[i] = data.AsLongBE(index);
            index += sizeof(long);
        }
        
        return index;
    }

    public void Serialize(Stream stream, bool named = true)
    {
        if (named) WriteName(stream);
        stream.WriteIntBE(Value.Length);
        foreach (var v in Value)
        {
            stream.WriteLongBE(v);
        }
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append(TagType.LongArray.Name);
        if (this.Name is null)
        {
            sb.Append("(None): ");
        }else
        {
            sb.Append($"('{this.Name}'): ");
        }
        sb.Append($"[{Value.Length} longs]");
        
        return sb.ToString();
    }
}