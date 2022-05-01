namespace Moonpie.NBT.Exceptions;

public class UnsupportedTagTypeException : Exception
{
    public TagType TagType { get; private set; }
    public UnsupportedTagTypeException(TagType type) : base()
    {
        this.TagType = type;   
    }

    public override string Message => $"Tag type {TagType} is not supported";
}