using System.Text;
using Moonpie.NBT.Utils;

namespace Moonpie.NBT.Tags;

public class NBTFloat : NBTNamedBase, INBTSerializable
{
    public NBTFloat() : base(TagType.Float)
    {
        
    }
    
    public float Value { get; set; }

    public int Deserialize(Span<byte> data, int index, bool named = true)
    {
        if (named)
        {
            index = ReadName(data, index);
        }
        Value = data.AsFloatBE(index);
        return index + sizeof(float);
    }

    public Span<byte> Serialize(bool named = true)
    {
        throw new NotImplementedException();
    }
    
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append(TagType.Float.Name);
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