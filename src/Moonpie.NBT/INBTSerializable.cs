namespace Moonpie.NBT;

internal interface INBTSerializable
{
    public int Deserialize(Span<byte> data, int index, bool named = true);
    public void Serialize(Stream stream, bool named = true);
}