namespace Repara.Shared.Exceptions;

/// <summary>
/// Representa uma exceção personalizada para erros internos do servidor.
/// </summary>
public class InternalServerErrorException : Exception
{
    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="InternalServerErrorException"/>
    /// com uma mensagem padrão indicando um erro interno no servidor.
    /// </summary>
    public InternalServerErrorException() 
        : base("erro interno no servidor")
    {
    }
    
    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="InternalServerErrorException"/>
    /// com uma mensagem personalizada.
    /// </summary>
    /// <param name="message">A mensagem de erro personalizada.</param>
    public InternalServerErrorException(string message) 
        : base(message)
    {
    }

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="InternalServerErrorException"/>
    /// com uma mensagem personalizada e uma exceção interna.
    /// </summary>
    /// <param name="message">A mensagem de erro personalizada.</param>
    /// <param name="inner">A exceção que causou o erro atual.</param>
    public InternalServerErrorException(string message, Exception inner) 
        : base(message, inner)
    {
    }
}