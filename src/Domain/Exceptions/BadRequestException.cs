namespace BevMan.Domain.Exceptions;

[Serializable]
public class BadRequestException : Exception
{
    public BadRequestException()
    {
    }

    public BadRequestException(string message) : base(message)
    {
    }
}
