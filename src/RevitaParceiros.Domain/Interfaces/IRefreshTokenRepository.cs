using RevitaParceiros.Domain.Entities;

namespace RevitaParceiros.Domain.Interfaces;

/// <summary>
/// Interface para o repositório de tokens de atualização.
/// </summary>
public interface IRefreshTokenRepository
{
    Task<TokensAtualizacao?> GetByTokenAsync(string token, CancellationToken ct = default);
    Task AddAsync(TokensAtualizacao refreshToken, CancellationToken ct = default);
    Task RevokeAllByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task UpdateAsync(TokensAtualizacao refreshToken, CancellationToken ct = default);
}
