using Microsoft.EntityFrameworkCore;
using RevitaParceiros.Application.Common.Interfaces;
using RevitaParceiros.Domain.Entities;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Infra.Persistence.Repositories;

public sealed class RefreshTokenRepository(
    DatabaseContext context,
    IDateTimeProvider dateTimeProvider)
    : IRefreshTokenRepository
{
    public Task<TokensAtualizacao?> GetByTokenAsync(string token, CancellationToken ct = default)
    {
        return context.TokensAtualizacao
            .FirstOrDefaultAsync(rt => rt.Token == token, ct);
    }

    public async Task AddAsync(TokensAtualizacao refreshToken, CancellationToken ct = default)
    {
        await context.TokensAtualizacao.AddAsync(refreshToken, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task RevokeAllByUserIdAsync(Guid userId, CancellationToken ct = default)
    {
        var tokens = await context.TokensAtualizacao
            .Where(rt => rt.UsuarioId == userId && rt.RevogadoEm == null && rt.ExpiraEm > dateTimeProvider.UtcNow)
            .ToListAsync(ct);

        foreach (var token in tokens)
        {
            token.RevogadoEm = dateTimeProvider.UtcNow;
        }

        if (tokens.Count != 0)
        {
            await context.SaveChangesAsync(ct);
        }
    }

    public async Task UpdateAsync(TokensAtualizacao refreshToken, CancellationToken ct = default)
    {
        context.TokensAtualizacao.Update(refreshToken);
        await context.SaveChangesAsync(ct);
    }
}
