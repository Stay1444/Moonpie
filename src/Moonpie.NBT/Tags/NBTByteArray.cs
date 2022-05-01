using System.Text;
using Moonpie.NBT.Utils;

namespace Moonpie.NBT.Tags;

public class NBTByteArray : NBTNamedBase, INBTSerializable
{
    internal NBTByteArray() : base(TagType.ByteArray)
    {
        
    }
    
    public NBTByteArray(string name, byte[] value) : base(TagType.ByteArray)
    {
        Name = name;
        Value = value;
    }

    public byte[] Value { get; set; } = Array.Empty<byte>();
    public int Deserialize(Span<byte> data, int index, bool named = true)
    {
        if (named)
        {
            index = ReadName(data, index);
        }

        var length = data.AsIntBE(index);
        index += sizeof(int);
        
        Value = new byte[length];
        
        for (var i = 0; i < length; i++)
        {
            Value[i] = data[index + i];
        }
        
        return index + length;
    }

    public void Serialize(Stream stream, bool named = true)
    {
        if (named) WriteName(stream);
        
        stream.WriteIntBE(Value.Length);
        
        foreach (var b in Value)
        {
            stream.WriteByte(b);
        }
    }


    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append(TagType.ByteArray.Name);
        if (this.Name is null)
        {
            sb.Append("(None): ");
        }else
        {
            sb.Append($"('{this.Name}'): ");
        }
        sb.Append($"[{Value.Length} bytes]");
        
        return sb.ToString();
    }
}