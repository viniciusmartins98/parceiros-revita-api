using Mediator;
using RevitaParceiros.Application.Features.Points.Dtos;

namespace RevitaParceiros.Application.Features.Points.SearchProfiles;

public record SearchProfilesQuery(string? Term) : IRequest<List<ProfileDto>>;
