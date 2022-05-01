using System.Text;
using Moonpie.NBT.Utils;

namespace Moonpie.NBT.Tags;

public class NBTInt : NBTNamedBase, INBTSerializable
{
    internal NBTInt() : base(TagType.Int)
    {
        
    }
    
    public NBTInt(string name, int value) : base(TagType.Int)
    {
        Name = name;
        Value = value;
    }
    
    public int Value { get; set; }

    public int Deserialize(Span<byte> data, int index, bool named = true)
    {
        if (named)
        {
            index = ReadName(data, index);
        }

        Value = data.AsIntBE(index);
        return index + sizeof(int);
    }

    public void Serialize(Stream stream, bool named = true)
    {
        if (named) WriteName(stream);
        stream.WriteIntBE(Value);
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append(TagType.Int.Name);
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