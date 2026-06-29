using Mediator;
using RevitaParceiros.Application.Features.Points.Dtos;
using RevitaParceiros.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RevitaParceiros.Application.Features.Points.GetRecentRedemptions;

public class GetRecentRedemptionsQueryHandler(IResgatesRepository resgatesRepository) : IRequestHandler<GetRecentRedemptionsQuery, IEnumerable<RecentRedemptionDto>>
{
    public async ValueTask<IEnumerable<RecentRedemptionDto>> Handle(GetRecentRedemptionsQuery request, CancellationToken cancellationToken)
    {
        var recent = await resgatesRepository.GetRecentAsync(request.Limit, cancellationToken);
        
        return recent.Select(r =>
        {
            var profileId = r.ClienteId ?? r.ParceiroId ?? Guid.Empty;
            var profileName = r.Cliente?.Usuario?.Nome ?? r.Parceiro?.Usuario?.Nome ?? "Desconhecido";
            
            return new RecentRedemptionDto(
                r.Id,
                r.CriadoEm,
                profileId,
                profileName,
                r.PontosResgatados,
                r.ValorMonetario
            );
        });
    }
}
