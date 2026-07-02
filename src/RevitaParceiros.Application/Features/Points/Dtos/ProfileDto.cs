using RevitaParceiros.Domain.Enums;

namespace RevitaParceiros.Application.Features.Points.Dtos;

public record ProfileDto(Guid Id, string Name, string Email, PerfilUsuarioEnum Type, int Points);
