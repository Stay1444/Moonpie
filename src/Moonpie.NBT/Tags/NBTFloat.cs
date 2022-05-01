using System.Text;
using Moonpie.NBT.Utils;

namespace Moonpie.NBT.Tags;

public class NBTFloat : NBTNamedBase, INBTSerializable
{
    internal NBTFloat() : base(TagType.Float)
    {
        
    }
    
    public NBTFloat(string name, float value) : base(TagType.Float)
    {
        Name = name;
        Value = value;
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

    public void Serialize(Stream stream, bool named = true)
    {
        if (named) WriteName(stream);
        stream.WriteFloatBE(Value);
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