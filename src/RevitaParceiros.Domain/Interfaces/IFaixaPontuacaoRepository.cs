using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RevitaParceiros.Domain.Entities;

namespace RevitaParceiros.Domain.Interfaces;

public interface IFaixaPontuacaoRepository
{
    Task AddRangeAsync(IEnumerable<FaixasPontuacao> faixas, CancellationToken cancellationToken = default);
}
