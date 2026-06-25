namespace RevitaParceiros.Domain.Exceptions;

/// <summary>
/// Exceção lançada quando um recurso solicitado não é encontrado.
/// </summary>
public sealed class NotFoundException : Exception
{
    public NotFoundException(string message)
        : base(message)
    {
    }

    public NotFoundException(string entityName, object key)
        : base($"{entityName} com identificador '{key}' não foi encontrado.")
    {
    }
}
