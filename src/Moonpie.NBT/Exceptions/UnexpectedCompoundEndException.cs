namespace Moonpie.NBT.Exceptions;

public class UnexpectedCompoundEndException : Exception
{
    public override string Message => "Unexpected end of compound tag.";
}