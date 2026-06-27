using RevitaParceiros.Domain.Entities;

namespace RevitaParceiros.Domain.Interfaces;

public interface IRegrasPontuacaoRepository
{
    Task<RegrasPontuacao?> GetActiveConfigAsync(CancellationToken cancellationToken = default);
    Task AddAsync(RegrasPontuacao config, CancellationToken cancellationToken = default);
    Task UpdateAsync(RegrasPontuacao config, CancellationToken cancellationToken = default);
}
