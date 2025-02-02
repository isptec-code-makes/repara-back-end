namespace Repara.Shared.Exceptions;

public class BadRequestException : Exception
{
    public BadRequestException() : base("requisição mal formada ou inválida")
    {
    }
    public BadRequestException(string message) : base(message)
    {
    }

    public BadRequestException(string message, Exception inner) : base(message, inner)
    {
    }
}