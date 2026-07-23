using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RevitaParceiros.Domain.Entities;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Infra.Persistence.Repositories;

public sealed class FaixaPontuacaoRepository : IFaixaPontuacaoRepository
{
    private readonly DatabaseContext _context;

    public FaixaPontuacaoRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task AddRangeAsync(IEnumerable<FaixasPontuacao> faixas, CancellationToken cancellationToken = default)
    {
        await _context.FaixasPontuacao.AddRangeAsync(faixas, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
