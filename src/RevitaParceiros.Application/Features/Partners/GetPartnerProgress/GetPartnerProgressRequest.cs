using Mediator;

namespace RevitaParceiros.Application.Features.Partners.GetPartnerProgress;

public sealed record GetPartnerProgressRequest(Guid Id) : IRequest<PartnerProgressDto>;
