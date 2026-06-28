using Mediator;
using RevitaParceiros.Application.Common.Interfaces;
using RevitaParceiros.Application.Features.Auth.Login;
using RevitaParceiros.Domain.Entities;
using RevitaParceiros.Domain.Exceptions;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Application.Features.Auth.RefreshToken;

public sealed class RefreshTokenHandler(
    IRefreshTokenRepository refreshTokenRepository,
    IUsuarioRepository usuarioRepository,
    IJwtTokenService jwtTokenService,
    IDateTimeProvider dateTimeProvider)
    : IRequestHandler<RefreshTokenRequest, LoginResponse>
{
    public async ValueTask<LoginResponse> Handle(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var userId = jwtTokenService.GetUserIdFromExpiredToken(request.AccessToken);
        if (userId is null)
        {
            throw new UnauthorizedException("Token de acesso inválido.");
        }

        var storedRefreshToken = await refreshTokenRepository.GetByTokenAsync(request.RefreshToken, cancellationToken);

        if (storedRefreshToken is null || storedRefreshToken.UsuarioId != userId)
        {
            throw new UnauthorizedException("Refresh token inválido.");
        }

        if (storedRefreshToken.RevogadoEm is not null)
        {
            throw new UnauthorizedException("Refresh token foi revogado. Faça login novamente.");
        }

        if (storedRefreshToken.ExpiraEm < dateTimeProvider.UtcNow)
        {
            throw new UnauthorizedException("Refresh token expirado. Faça login novamente.");
        }

        var usuario = await usuarioRepository.GetByIdAsync(userId.Value, cancellationToken);
        if (usuario is null || !usuario.Ativo)
        {
            throw new UnauthorizedException("Usuário não encontrado ou inativo.");
        }

        // Revogar o token atual
        storedRefreshToken.RevogadoEm = dateTimeProvider.UtcNow;
        await refreshTokenRepository.UpdateAsync(storedRefreshToken, cancellationToken);

        // Obter perfil dinamicamente (como no LoginHandler)
        string perfil = "Cliente";
        var perfilProp = usuario.GetType().GetProperty("Perfil");
        if (perfilProp != null)
        {
            perfil = perfilProp.GetValue(usuario)?.ToString() ?? "Cliente";
        }
        else 
        {
            if (usuario.Id.ToString() == "00000000-0000-0000-0000-000000000000")
                perfil = "Administrador";
            else if (usuario.Clientes != null)
                perfil = "Cliente";
            else if (usuario.Parceiros != null)
                perfil = "Parceiro";
        }

        // Gerar novo par de tokens
        var newAccessToken = jwtTokenService.GenerateAccessToken(usuario.Id, usuario.Nome, perfil);
        var newRefreshTokenString = jwtTokenService.GenerateRefreshToken();
        var expiresAt = dateTimeProvider.UtcNow.AddDays(7); // Será sobrescrito no infra se precisar

        var newRefreshToken = new TokensAtualizacao
        {
            UsuarioId = usuario.Id,
            Token = newRefreshTokenString,
            ExpiraEm = expiresAt,
            CriadoEm = dateTimeProvider.UtcNow
        };

        await refreshTokenRepository.AddAsync(newRefreshToken, cancellationToken);

        return new LoginResponse(
            AccessToken: newAccessToken,
            RefreshToken: newRefreshTokenString,
            ExpiresAt: expiresAt,
            User: new UserInfo(
                usuario.Id,
                usuario.Parceiros?.Id,
                usuario.Clientes?.Id,
                usuario.Nome,
                perfil)
            );
    }
}
