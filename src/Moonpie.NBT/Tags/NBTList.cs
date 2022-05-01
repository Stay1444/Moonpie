using System.Text;
using Moonpie.NBT.Exceptions;
using Moonpie.NBT.Utils;

namespace Moonpie.NBT.Tags;

public class NBTList : NBTNamedBase, INBTSerializable
{
    internal NBTList() : base(TagType.TagList)
    {
        
    }
    
    public NBTList(string name, TagType type) : base(TagType.TagList)
    {
        this.Name = name;
        this.ListType = type;
    }
    
    public TagType ListType { get; private set; } = TagType.TagList;
    private List<NBTBase> _values = new List<NBTBase>();
    public IReadOnlyList<NBTBase> Values => _values;
    public int Deserialize(Span<byte> data, int index, bool named = true)
    {
        if (named)
        {
            index = ReadName(data, index);
        }
        
        ListType = TagType.FromValue(data[index]);
        index++;
        var length = data.Slice(index, sizeof(int)).AsIntBE();
        index += sizeof(int);
        
        for (var i = 0; i < length; i++)
        {
            var value = ListType.CreateInstance();
            
            _values.Add(value);
            
            if (value is INBTSerializable serializable)
            {
                index = serializable.Deserialize(data, index, false);
            }
            else
            {
                throw new UnsupportedTagTypeException(ListType);
            }
        }
        
        return index;
    }

    public void Serialize(Stream stream, bool named = true)
    {
        if (named) WriteName(stream);
        
        stream.WriteByte((byte) ListType);
        
        stream.WriteIntBE(Values.Count);
        
        foreach (var value in Values)
        {
            if (value is INBTSerializable serializable)
            {
                serializable.Serialize(stream, false);
            }else
            {
                throw new UnsupportedTagTypeException(value.Type);
            }
        } 
    }

    public override string ToString()
    {
        var result = new StringBuilder();
        result.Append(TagType.TagList.Name);
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
        get => _values[index];
        set => _values[index] = value;
    }
    
    public void Add(NBTBase value)
    {
        if (value.Type != ListType)
        {
            throw new ArgumentException($"Cannot add {value.Type} to {ListType} list");
        }
        
        _values.Add(value);
    }
    
    public void Remove(NBTBase value)
    {
        _values.Remove(value);
    }
    
    public void Clear()
    {
        _values.Clear();
    }
    
    public bool Contains(NBTBase value)
    {
        return _values.Contains(value);
    }
    
    public int IndexOf(NBTBase value)
    {
        return _values.IndexOf(value);
    }
    
    public void Insert(int index, NBTBase value)
    {
        _values.Insert(index, value);
    }
    
    public void RemoveAt(int index)
    {
        _values.RemoveAt(index);
    }
    
    public IEnumerator<NBTBase> GetEnumerator()
    {
        return _values.GetEnumerator();
    }
}