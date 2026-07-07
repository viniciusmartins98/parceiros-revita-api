using RevitaParceiros.Domain.Entities;

namespace RevitaParceiros.Domain.Interfaces;

/// <summary>
/// Interface para o repositório de usuários, definindo operações específicas de busca e verificação.
/// </summary>
public interface IUsuarioRepository
{
    Task<Usuarios?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Usuarios?> GetByEmailAsync(string email, CancellationToken ct = default);
    Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default);
    Task<List<Usuarios>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(Usuarios usuario, CancellationToken ct = default);
    Task UpdateAsync(Usuarios usuario, CancellationToken ct = default);
    Task DeleteAsync(Usuarios usuario, CancellationToken ct = default);
}
