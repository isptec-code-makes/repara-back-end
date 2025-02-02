namespace Repara.Shared.Exceptions;

/// <summary>
/// Exceção personalizada para indicar que um recurso ou conteúdo não foi encontrado.
/// </summary>
public class NotFoundException : Exception
{
    /// <summary>
    /// Construtor padrão da exceção que utiliza uma mensagem padrão: "content not found".
    /// </summary>
    public NotFoundException() : base("content not found")
    {
    }

    /// <summary>
    /// Construtor que permite especificar uma mensagem personalizada para a exceção.
    /// </summary>
    /// <param name="message">Mensagem descritiva do erro.</param>
    public NotFoundException(string message) : base(message)
    {
    }

    /// <summary>
    /// Construtor que permite especificar uma mensagem personalizada e uma exceção interna que causou este erro.
    /// </summary>
    /// <param name="message">Mensagem descritiva do erro.</param>
    /// <param name="inner">Exceção interna que originou este erro.</param>
    public NotFoundException(string message, Exception inner) : base(message, inner)
    {
    }
}