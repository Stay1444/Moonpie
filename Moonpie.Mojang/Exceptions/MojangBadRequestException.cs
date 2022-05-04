using Moonpie.Mojang.Models;

namespace Moonpie.Mojang.Exceptions;

public class MojangBadRequestException : Exception
{
    public ErrorModel Error { get; }
    public MojangBadRequestException(ErrorModel error)
    {
        this.Error = error;
    }

    public override string Message => Error.Error + ": " + Error.ErrorMessage;
}