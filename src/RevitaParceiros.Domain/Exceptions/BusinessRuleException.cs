namespace RevitaParceiros.Domain.Exceptions;

/// <summary>
/// Exceção lançada quando uma regra de negócio é violada.
/// </summary>
public sealed class BusinessRuleException : Exception
{
    public BusinessRuleException(string message)
        : base(message)
    {
    }
}
