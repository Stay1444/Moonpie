using System.Text;
using Moonpie.NBT.Utils;

namespace Moonpie.NBT.Tags;

public class NBTDouble : NBTNamedBase, INBTSerializable
{
    internal NBTDouble() : base(TagType.Double)
    {
        
    }
    
    public NBTDouble(string name, double value) : base(TagType.Double)
    {
        Name = name;
        Value = value;
    }
    
    public double Value { get; set; }

    public int Deserialize(Span<byte> data, int index, bool named = true)
    {
        if (named)
        {
            index = ReadName(data, index);
        }

        Value = data.AsDoubleBE(index);
        return index + sizeof(double);
    }

    public void Serialize(Stream stream, bool named = true)
    {
        if (named) WriteName(stream);
        stream.WriteDoubleBE(Value);
    }


    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append(TagType.Double.Name);
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