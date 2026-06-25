namespace RevitaParceiros.Domain.Exceptions;

/// <summary>
/// Exceção lançada quando o usuário não possui permissão para acessar o recurso.
/// </summary>
public sealed class UnauthorizedException : Exception
{
    public UnauthorizedException(string message)
        : base(message)
    {
    }

    public UnauthorizedException()
        : base("Você não possui permissão para realizar esta ação.")
    {
    }
}
