using Microsoft.EntityFrameworkCore;
using RevitaParceiros.Domain.Entities;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Infra.Persistence.Repositories;

public sealed class ExtratoPontosRepository(DatabaseContext context) : IExtratoPontosRepository
{
    public async Task<IReadOnlyCollection<ExtratoPontos>> GetByClientIdAsync(Guid clientId, CancellationToken cancellationToken = default)
    {
        return await context.ExtratoPontos
            .AsNoTracking()
            .Where(e => e.ClienteId == clientId)
            .OrderByDescending(e => e.CriadoEm)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<ExtratoPontos>> GetByPartnerIdAsync(Guid partnerId, CancellationToken cancellationToken = default)
    {
        return await context.ExtratoPontos
            .AsNoTracking()
            .Where(e => e.ParceiroId == partnerId)
            .OrderByDescending(e => e.CriadoEm)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(ExtratoPontos extrato, CancellationToken cancellationToken = default)
    {
        await context.ExtratoPontos.AddAsync(extrato, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<DateTime?> GetPartnerLastRedemptionDateAsync(Guid partnerId, CancellationToken cancellationToken = default)
    {
        var lastRedemption = await context.ExtratoPontos
            .AsNoTracking()
            .Where(e => e.ParceiroId == partnerId && e.TipoTransacao == RevitaParceiros.Domain.Enums.TipoTransacaoPontosEnum.Resgate)
            .OrderByDescending(e => e.CriadoEm)
            .FirstOrDefaultAsync(cancellationToken);

        return lastRedemption?.CriadoEm;
    }
}
