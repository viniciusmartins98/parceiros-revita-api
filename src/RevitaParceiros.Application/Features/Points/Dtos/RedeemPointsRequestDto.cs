using RevitaParceiros.Domain.Enums;

namespace RevitaParceiros.Application.Features.Points.Dtos;

public record RedeemPointsRequestDto(Guid ProfileId, PerfilUsuarioEnum ProfileType, int Points);
