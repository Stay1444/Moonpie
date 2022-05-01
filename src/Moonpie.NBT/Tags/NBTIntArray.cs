using System.Text;
using Moonpie.NBT.Utils;

namespace Moonpie.NBT.Tags;

public class NBTIntArray : NBTNamedBase, INBTSerializable
{
    public NBTIntArray() : base(TagType.IntArray)
    {
        
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

    public Span<byte> Serialize(bool named = true)
    {
        throw new NotImplementedException();
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