namespace Repara.Shared.Exceptions;

/// <summary>
/// Exceção personalizada para indicar que o acesso a um recurso ou conteúdo foi proibido.
/// </summary>
public class ForbiddenException : Exception
{
    /// <summary>
    /// Construtor padrão da exceção que utiliza uma mensagem padrão: "acesso proibido ao conteúdo".
    /// </summary>
    public ForbiddenException() : base("acesso proibido ao conteúdo")
    {
    }

    /// <summary>
    /// Construtor que permite especificar uma mensagem personalizada para a exceção.
    /// </summary>
    /// <param name="message">Mensagem descritiva do erro.</param>
    public ForbiddenException(string message) : base(message)
    {
    }

    /// <summary>
    /// Construtor que permite especificar uma mensagem personalizada e uma exceção interna que causou este erro.
    /// </summary>
    /// <param name="message">Mensagem descritiva do erro.</param>
    /// <param name="inner">Exceção interna que originou este erro.</param>
    public ForbiddenException(string message, Exception inner) : base(message, inner)
    {
    }
}