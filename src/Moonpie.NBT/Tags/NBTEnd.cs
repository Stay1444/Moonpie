using System.Text;

namespace Moonpie.NBT.Tags;

public class NBTEnd : NBTBase
{
    internal NBTEnd() : base(TagType.End)
    {
        
    }
    
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append(TagType.End.Name);
        return sb.ToString();
    }
}