namespace Moonpie.Protocol.Protocol;

public class NBT
{
    public NBTTagTypes Type;
    public object? Value;
    
    public NBT(NBTTagTypes type, object? value)
    {
        Type = type;
        Value = value;
    }
    
}