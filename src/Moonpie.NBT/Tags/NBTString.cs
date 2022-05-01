using System.Text;
using Moonpie.NBT.Utils;

namespace Moonpie.NBT.Tags;

public class NBTString : NBTNamedBase, INBTSerializable
{
    public NBTString() : base(TagType.String)
    {
        
    }
    public string Value { get; set; } = String.Empty;
    public int Deserialize(Span<byte> data, int index, bool named = true)
    {
        if (named)
        {
            index = ReadName(data, index);
        }
        
        var length = data.AsUShortBE(index);
        index += sizeof(ushort);
        
        this.Value = data.AsString(index, length);
        index += length;

        return index;
    }

    public Span<byte> Serialize(bool named = true)
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append(TagType.String.Name);
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