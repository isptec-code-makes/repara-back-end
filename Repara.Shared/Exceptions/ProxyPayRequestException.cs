namespace Repara.Shared.Exceptions;

public class ProxyPayRequestException: Exception
{
    public ProxyPayRequestException() : base("erro na comunicação com a proxypay")
    {
    }
    public ProxyPayRequestException(string message) : base(message)
    {
    }

    public ProxyPayRequestException(string message, Exception inner) : base(message, inner)
    {
    }
}