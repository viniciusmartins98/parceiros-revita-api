using Mediator;
using RevitaParceiros.Application.Common.Interfaces;
using RevitaParceiros.Domain.Entities;
using RevitaParceiros.Domain.Exceptions;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Application.Features.Auth.Login;

public sealed class LoginHandler(
    IUsuarioRepository usuarioRepository,
    IRefreshTokenRepository refreshTokenRepository,
    IJwtTokenService jwtTokenService,
    IPasswordHasher passwordHasher,
    IDateTimeProvider dateTimeProvider)
    : IRequestHandler<LoginRequest, LoginResponse>
{
    public async ValueTask<LoginResponse> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        Usuarios? usuario;

        usuario = await usuarioRepository.GetByEmailAsync(request.Login, cancellationToken);

        if (usuario is null || !passwordHasher.Verify(request.Senha, usuario.SenhaHash))
        {
            throw new UnauthorizedException("Login ou senha incorretos.");
        }

        if (!usuario.Ativo)
        {
            throw new UnauthorizedException("Usuário inativo. Entre em contato com o suporte.");
        }


        var employeeId = usuario?.Funcionarios?.Id;
        var partnerId = usuario?.Parceiros?.Id;
        var clientId = usuario?.Clientes?.Id;

        var accessToken = jwtTokenService.GenerateAccessToken(usuario.Id, usuario.Nome, usuario.Perfil.ToString(), employeeId, partnerId, clientId);
        var refreshTokenString = jwtTokenService.GenerateRefreshToken();
        var expiresAt = dateTimeProvider.UtcNow.AddDays(7); // Vai ser sobreposto na infra baseado no appsettings

        var refreshToken = new TokensAtualizacao
        {
            UsuarioId = usuario.Id,
            Token = refreshTokenString,
            ExpiraEm = expiresAt,
            CriadoEm = dateTimeProvider.UtcNow
        };

        // Revoga os antigos para manter o banco limpo e apenas uma sessão por usuário (opcional, mas bom padrão)
        await refreshTokenRepository.RevokeAllByUserIdAsync(usuario.Id, cancellationToken);

        await refreshTokenRepository.AddAsync(refreshToken, cancellationToken);

        return new LoginResponse(
            AccessToken: accessToken,
            RefreshToken: refreshTokenString,
            ExpiresAt: expiresAt,
            User: new UserInfo(
                usuario.Id,
                employeeId,
                partnerId,
                clientId,
                usuario.Nome,
                usuario.Perfil.ToString())
            );
    }
}
