namespace Repara.Shared.Exceptions;

public class ConflictRequestException : Exception
{
    public ConflictRequestException() : base("o recurso a ser criado já existe")
    {
    }
    public ConflictRequestException(string message) : base(message)
    {
    }

    public ConflictRequestException(string message, Exception inner) : base(message, inner)
    {
    }
}