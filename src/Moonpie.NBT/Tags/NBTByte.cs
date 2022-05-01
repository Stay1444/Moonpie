using System.Text;

namespace Moonpie.NBT.Tags;

public class NBTByte : NBTNamedBase, INBTSerializable
{
    internal NBTByte() : base(TagType.Byte)
    {
        
    }
    
    public NBTByte(string name, byte value) : base(TagType.Byte)
    {
        Name = name;
        Value = value;
    }

    public byte Value { get; set; }
    
    public int Deserialize(Span<byte> data, int index, bool named = true)
    {
        if (named)
        {
            index = ReadName(data, index);
        }
        
        Value = data[index];
        return index + sizeof(byte);
    }

    public void Serialize(Stream stream, bool named = true)
    {
        if (named) WriteName(stream);
        stream.WriteByte(Value);
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append(TagType.Byte.Name);
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