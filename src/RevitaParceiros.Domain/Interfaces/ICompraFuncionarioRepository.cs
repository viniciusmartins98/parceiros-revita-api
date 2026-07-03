using RevitaParceiros.Domain.Entities;

namespace RevitaParceiros.Domain.Interfaces;

/// <summary>
/// Repositório para operações sobre a entidade ComprasFuncionarios.
/// </summary>
public interface ICompraFuncionarioRepository
{
    Task<IReadOnlyCollection<ComprasFuncionarios>> GetByFuncionarioIdAsync(Guid funcionarioId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<ComprasFuncionarios>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(ComprasFuncionarios compra, CancellationToken cancellationToken = default);
}
