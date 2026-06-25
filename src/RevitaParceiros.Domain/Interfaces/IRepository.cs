using RevitaParceiros.Domain.Common;

namespace RevitaParceiros.Domain.Interfaces;

/// <summary>
/// Interface genérica base para repositórios.
/// As implementações concretas ficam na camada de Infra.
/// </summary>
/// <typeparam name="T">Tipo da entidade, deve herdar de <see cref="BaseEntity"/>.</typeparam>
public interface IRepository<T> where T : BaseEntity
{
    /// <summary>
    /// Obtém uma entidade pelo seu identificador.
    /// </summary>
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém todas as entidades.
    /// </summary>
    Task<IReadOnlyCollection<T>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Adiciona uma nova entidade.
    /// </summary>
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Atualiza uma entidade existente.
    /// </summary>
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove uma entidade.
    /// </summary>
    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
}
