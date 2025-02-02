namespace Repara.Shared.Exceptions;

public class UnsupportedMediaTypeException: Exception
{
    public UnsupportedMediaTypeException() :base("extensão de arquivo não suportado") { }
    public UnsupportedMediaTypeException(string message) :base("a extensão do ficheiro não é válida para a tarefa solicitada, " + message) { }
    public UnsupportedMediaTypeException(string message, Exception inner) : base("extensão de arquivo não suportado,", inner) { }
}