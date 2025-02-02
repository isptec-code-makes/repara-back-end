namespace Repara.Shared.Exceptions;

/// <summary>
/// Exceção personalizada para indicar que o usuário não está autorizado a acessar um recurso ou conteúdo.
/// </summary>
public class UnauthorizedException : Exception
{
    /// <summary>
    /// Construtor padrão da exceção que utiliza uma mensagem padrão: "conteúdo não encontrado".
    /// </summary>
    public UnauthorizedException() : base("conteúdo não encontrado")
    {
    }

    /// <summary>
    /// Construtor que permite especificar uma mensagem personalizada para a exceção.
    /// </summary>
    /// <param name="message">Mensagem descritiva do erro.</param>
    public UnauthorizedException(string message) : base(message)
    {
    }

    /// <summary>
    /// Construtor que permite especificar uma mensagem personalizada e uma exceção interna que causou este erro.
    /// </summary>
    /// <param name="message">Mensagem descritiva do erro.</param>
    /// <param name="inner">Exceção interna que originou este erro.</param>
    public UnauthorizedException(string message, Exception inner) : base(message, inner)
    {
    }
}