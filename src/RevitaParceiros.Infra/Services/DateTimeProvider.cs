using RevitaParceiros.Application.Common.Interfaces;

namespace RevitaParceiros.Infra.Services;

/// <summary>
/// Implementação concreta de <see cref="IDateTimeProvider"/>.
/// </summary>
public sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
