namespace Moonpie.NBT;

public abstract class NBTBase
{
    public TagType Type { get; }

    protected NBTBase(TagType type)
    {
        Type = type;
    }
    
    public T As<T>() where T : NBTBase
    {
        return this as T ?? throw new InvalidCastException($"Cannot cast {this.GetType().Name} to {typeof(T).Name}");
    }
}