using Microsoft.EntityFrameworkCore;
using RevitaParceiros.Domain.Entities;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Infra.Persistence.Repositories;

public class RegrasPontuacaoRepository : IRegrasPontuacaoRepository
{
    private readonly DatabaseContext _context;

    public RegrasPontuacaoRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<RegrasPontuacao?> GetActiveConfigAsync(CancellationToken cancellationToken = default)
    {
        return await _context.RegrasPontuacao
            .FirstOrDefaultAsync(x => x.Ativo, cancellationToken);
    }

    public async Task AddAsync(RegrasPontuacao config, CancellationToken cancellationToken = default)
    {
        await _context.RegrasPontuacao.AddAsync(config, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(RegrasPontuacao config, CancellationToken cancellationToken = default)
    {
        _context.RegrasPontuacao.Update(config);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
