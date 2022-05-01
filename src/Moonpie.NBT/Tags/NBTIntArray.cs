using System.Text;
using Moonpie.NBT.Utils;

namespace Moonpie.NBT.Tags;

public class NBTIntArray : NBTNamedBase, INBTSerializable
{
    internal NBTIntArray() : base(TagType.IntArray)
    {
        
    }
    
    public NBTIntArray(string name, int[] value) : base(TagType.IntArray)
    {
        Value = value;
        Name = name;
    }
    
    public int[] Value { get; set; } = Array.Empty<int>();
    
    public int Deserialize(Span<byte> data, int index, bool named = true)
    {
        if (named)
        {
            index = ReadName(data, index);
        }

        var length = data.AsIntBE(index);
        index += sizeof(int);
        
        Value = new int[length];
        
        for (var i = 0; i < length; i++)
        {
            Value[i] = data.AsIntBE(index);
            index += sizeof(int);
        }
        
        return index;
    }

    public void Serialize(Stream stream, bool named = true)
    {
        if (named) WriteName(stream);
        
        stream.WriteIntBE(Value.Length);
        
        foreach (var i in Value)
        {
            stream.WriteIntBE(i);
        }
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append(TagType.IntArray.Name);
        if (this.Name is null)
        {
            sb.Append("(None): ");
        }else
        {
            sb.Append($"('{this.Name}'): ");
        }
        sb.Append($"[{Value.Length} ints]");
        
        return sb.ToString();
    }
}