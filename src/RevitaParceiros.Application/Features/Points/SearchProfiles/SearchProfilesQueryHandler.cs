using Mediator;
using RevitaParceiros.Application.Features.Points.Dtos;
using RevitaParceiros.Domain.Enums;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Application.Features.Points.SearchProfiles;

public class SearchProfilesQueryHandler(
    IClienteRepository clienteRepository,
    IParceiroRepository parceiroRepository) : IRequestHandler<SearchProfilesQuery, List<ProfileDto>>
{
    public async ValueTask<List<ProfileDto>> Handle(SearchProfilesQuery request, CancellationToken cancellationToken)
    {
        var term = request.Term?.Trim().ToLowerInvariant();

        var clients = await clienteRepository.GetAllAsync(cancellationToken);
        var partners = await parceiroRepository.GetAllAsync(cancellationToken);

        var profiles = new List<ProfileDto>();

        foreach (var client in clients)
        {
            if (string.IsNullOrEmpty(term) || client.Usuario.Nome.ToLowerInvariant().Contains(term))
            {
                profiles.Add(new ProfileDto(client.Id, client.Usuario.Nome, client.Usuario.Email, PerfilUsuarioEnum.Cliente, client.TotalPontos));
            }
        }

        foreach (var partner in partners)
        {
            if (string.IsNullOrEmpty(term) || partner.Usuario.Nome.ToLowerInvariant().Contains(term))
            {
                profiles.Add(new ProfileDto(partner.Id, partner.Usuario.Nome, partner.Usuario.Email, PerfilUsuarioEnum.Parceiro, partner.TotalPontos));
            }
        }

        return profiles.OrderBy(p => p.Name).ToList();
    }
}
