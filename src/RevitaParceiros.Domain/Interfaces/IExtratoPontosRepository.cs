using RevitaParceiros.Domain.Entities;

namespace RevitaParceiros.Domain.Interfaces;

public interface IExtratoPontosRepository
{
    Task<IReadOnlyCollection<ExtratoPontos>> GetByClientIdAsync(Guid clientId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<ExtratoPontos>> GetByPartnerIdAsync(Guid partnerId, CancellationToken cancellationToken = default);
}
