using Mediator;
using RevitaParceiros.Application.Common.Interfaces;
using RevitaParceiros.Domain.Exceptions;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Application.Features.Auth.Logout;

public sealed class LogoutHandler(
    IRefreshTokenRepository refreshTokenRepository,
    IDateTimeProvider dateTimeProvider)
    : IRequestHandler<LogoutRequest, Unit>
{
    public async ValueTask<Unit> Handle(LogoutRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
            throw new BusinessRuleException("O Refresh Token não pode estar vazio.");

        var storedRefreshToken = await refreshTokenRepository.GetByTokenAsync(request.RefreshToken, cancellationToken);

        // Se o token existe e não foi revogado ainda, nós revogamos
        if (storedRefreshToken is not null && storedRefreshToken.RevogadoEm is null)
        {
            storedRefreshToken.RevogadoEm = dateTimeProvider.UtcNow;
            await refreshTokenRepository.UpdateAsync(storedRefreshToken, cancellationToken);
        }

        return Unit.Value;
    }
}
