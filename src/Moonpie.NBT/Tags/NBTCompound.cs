using System.Text;
using Moonpie.NBT.Exceptions;

namespace Moonpie.NBT.Tags;

public class NBTCompound : NBTNamedBase, INBTSerializable
{
    internal NBTCompound() : base(TagType.Compound)
    {
        
    }

    public NBTCompound(string name) : base(TagType.Compound)
    {
        this.Name = name;
    }    
    
    public List<NBTBase> Children { get; set; } = new List<NBTBase>();
    public int Deserialize(Span<byte> data, int index, bool named = true)
    {
        if (named)
        {
            index = ReadName(data, index);
        }

        while (true)
        {
            if (data.Length <= index)
            {
                throw new UnexpectedCompoundEndException();
            }
            
            var tagType = TagType.FromValue(data[index]);
            index++;
            
            var instance = tagType.CreateInstance();
            
            
            if (instance is NBTEnd)
            {
                break;
            }
            Children.Add(instance);

            if (instance is INBTSerializable serializable)
            {
                index = serializable.Deserialize(data, index);
            }
            else
            {
                throw new UnsupportedTagTypeException(tagType);
            }
        }
        
        return index;
    }

    public void Serialize(Stream stream, bool named = true)
    {
        if (named) WriteName(stream);
        
        foreach (var child in Children)
        {
            stream.WriteByte((byte) child.Type.Value);
            if (child is INBTSerializable serializable)
            {
                serializable.Serialize(stream);
            }else
            {
                throw new UnsupportedTagTypeException(child.Type);
            }
        }
        
        stream.WriteByte((byte) TagType.End.Value);
    }

    public override string ToString()
    {
        var result = new StringBuilder();
        result.Append(TagType.Compound.Name);
        if (this.Name is null)
        {
            result.Append("(None): ");
        }else
        {
            result.Append($"('{this.Name}'): ");
        }

        if (Children.Count > 1)
        {
            result.Append($"{Children.Count} entries");
        }else if (Children.Count == 1)
        {
            result.Append($"1 entry");
        }else
        {
            result.Append($"{Children.Count} entries");
        }

        result.AppendLine();
        
        result.AppendLine("{");
        
        foreach (var child in Children)
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
    
    public NBTBase this[string name]
    {
        get
        {
            foreach (var child in Children)
            {
                if (child is NBTNamedBase named && named.Name == name)
                {
                    return child;
                }
            }
            
            throw new KeyNotFoundException("No child with name " + name);
        }
        
        set
        {
            for (var i = 0; i < Children.Count; i++)
            {
                if (Children[i] is NBTNamedBase named && named.Name == name)
                {
                    Children[i] = value;
                    return;
                }
            }
            
            Children.Add(value);
        }
    }
    
}