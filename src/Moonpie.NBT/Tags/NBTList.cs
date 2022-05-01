using System.Text;
using Moonpie.NBT.Exceptions;
using Moonpie.NBT.Utils;

namespace Moonpie.NBT.Tags;

public class NBTList : NBTNamedBase, INBTSerializable
{
    public NBTList() : base(TagType.List)
    {
        
    }
    public List<NBTBase> Values { get; set; } = new List<NBTBase>();
    public int Deserialize(Span<byte> data, int index, bool named = true)
    {
        if (named)
        {
            index = ReadName(data, index);
        }
        
        var type = TagType.FromValue(data[index]);
        index++;
        var length = data.Slice(index, sizeof(int)).AsIntBE();
        index += sizeof(int);
        
        for (var i = 0; i < length; i++)
        {
            var value = type.CreateInstance();
            
            Values.Add(value);
            
            if (value is INBTSerializable serializable)
            {
                index = serializable.Deserialize(data, index, false);
            }
            else
            {
                throw new UnsupportedTagTypeException(type);
            }
        }
        
        return index;
    }

    public Span<byte> Serialize(bool named = true)
    {
        throw new NotImplementedException();
    }
    
    public override string ToString()
    {
        var result = new StringBuilder();
        result.Append(TagType.List.Name);
        if (this.Name is null)
        {
            result.Append("(None): ");
        }else
        {
            result.Append($"('{this.Name}'): ");
        }

        if (Values.Count > 1)
        {
            result.Append($"{Values.Count} entries");
        }else if (Values.Count == 1)
        {
            result.Append($"1 entry");
        }else
        {
            result.Append($"{Values.Count} entries");
        }

        result.AppendLine();
        
        result.AppendLine("{");
        
        foreach (var child in Values)
        {
            var childString = child.ToString() ?? "";
            foreach (var line in childString.Split(new[] {'\n'}, StringSplitOptions.None))
            {
                result.Append("    ");
                result.AppendLine(line);
            }
        }
        
        result.AppendLine("}");
        
        return result.ToString();
    }
    
    public NBTBase this[int index]
    {
        get => Values[index];
        set => Values[index] = value;
    }
}