using System.Runtime.CompilerServices;
using Ardalis.SmartEnum;
using Moonpie.NBT.Tags;

namespace Moonpie.NBT;

public class TagType : SmartEnum<TagType>
{
    public static readonly TagType End = new TagType("TAG_End", 0, typeof(NBTEnd));
    public static readonly TagType Byte = new TagType("TAG_Byte", 1, typeof(NBTByte));
    public static readonly TagType Short = new TagType("TAG_Short", 2, typeof(NBTShort));
    public static readonly TagType Int = new TagType("TAG_Int", 3, typeof(NBTInt));
    public static readonly TagType Long = new TagType("TAG_Long", 4, typeof(NBTLong));
    public static readonly TagType Float = new TagType("TAG_Float", 5, typeof(NBTFloat));
    public static readonly TagType Double = new TagType("TAG_Double", 6, typeof(NBTDouble));
    public static readonly TagType ByteArray = new TagType("TAG_ByteArray", 7, typeof(NBTByteArray));
    public static readonly TagType String = new TagType("TAG_String", 8, typeof(NBTString));
    public static readonly TagType TagList = new TagType("TAG_List", 9, typeof(NBTList));
    public static readonly TagType Compound = new TagType("TAG_Compound", 10, typeof(NBTCompound));
    public static readonly TagType IntArray = new TagType("TAG_IntArray", 11, typeof(NBTIntArray));
    public static readonly TagType LongArray = new TagType("TAG_LongArray", 12, typeof(NBTLongArray));
    
    
    public Type Type { get; }
    public NBTBase CreateInstance()
    {
        return (NBTBase)Activator.CreateInstance(Type)!;
    }

    private TagType(string name, int value, Type type) : base(name, value)
    {
        this.Type = type;
    }
}