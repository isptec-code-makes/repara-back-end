namespace Repara.Shared.Exceptions;

public class CreateIdentityUSerException : Exception
{
    public IEnumerable<string> Errors { get; }

    public CreateIdentityUSerException(IEnumerable<string> errors)
        : base("user registration errors.")
    {
        Errors = errors;
    }

    public override string ToString()
    {
        return $"{Message} Erros: {string.Join(", ", Errors)}";
    }
}