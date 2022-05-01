namespace Moonpie.NBT;

public interface INBTSerializable
{
    public int Deserialize(Span<byte> data, int index, bool named = true);
    public Span<byte> Serialize(bool named = true);
}