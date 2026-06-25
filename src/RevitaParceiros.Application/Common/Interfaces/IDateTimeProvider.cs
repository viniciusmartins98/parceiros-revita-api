namespace RevitaParceiros.Application.Common.Interfaces;

/// <summary>
/// Abstração para obtenção da data/hora atual (testabilidade).
/// </summary>
public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
