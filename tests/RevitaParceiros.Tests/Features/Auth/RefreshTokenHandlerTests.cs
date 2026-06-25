using FluentAssertions;
using NSubstitute;
using RevitaParceiros.Application.Common.Interfaces;
using RevitaParceiros.Application.Features.Auth.RefreshToken;
using RevitaParceiros.Domain.Entities;
using RevitaParceiros.Domain.Exceptions;
using RevitaParceiros.Domain.Interfaces;
using Xunit;

namespace RevitaParceiros.Tests.Features.Auth;

public class RefreshTokenHandlerTests
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly RefreshTokenHandler _handler;

    public RefreshTokenHandlerTests()
    {
        _refreshTokenRepository = Substitute.For<IRefreshTokenRepository>();
        _usuarioRepository = Substitute.For<IUsuarioRepository>();
        _jwtTokenService = Substitute.For<IJwtTokenService>();
        _dateTimeProvider = Substitute.For<IDateTimeProvider>();
        
        _handler = new RefreshTokenHandler(
            _refreshTokenRepository, 
            _usuarioRepository, 
            _jwtTokenService, 
            _dateTimeProvider);
    }

    [Fact]
    public async Task Refresh_WithValidTokens_ReturnsNewPair()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var request = new RefreshTokenRequest("expired_access_token", "valid_refresh_token");
        var storedToken = new TokensAtualizacao { UsuarioId = userId, Token = "valid_refresh_token", ExpiraEm = DateTime.UtcNow.AddDays(1) };
        var usuario = new Usuarios { Id = userId, Nome = "Teste", Ativo = true };

        _jwtTokenService.GetUserIdFromExpiredToken(request.AccessToken).Returns(userId);
        _refreshTokenRepository.GetByTokenAsync(request.RefreshToken, Arg.Any<CancellationToken>()).Returns(storedToken);
        _usuarioRepository.GetByIdAsync(userId, Arg.Any<CancellationToken>()).Returns(usuario);
        _dateTimeProvider.UtcNow.Returns(DateTime.UtcNow);
        _jwtTokenService.GenerateAccessToken(usuario.Id, usuario.Nome, Arg.Any<string>()).Returns("new_access_token");
        _jwtTokenService.GenerateRefreshToken().Returns("new_refresh_token");

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.AccessToken.Should().Be("new_access_token");
        result.RefreshToken.Should().Be("new_refresh_token");
        storedToken.RevogadoEm.Should().NotBeNull();
        await _refreshTokenRepository.Received(1).UpdateAsync(storedToken, Arg.Any<CancellationToken>());
        await _refreshTokenRepository.Received(1).AddAsync(Arg.Any<TokensAtualizacao>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Refresh_WithRevokedToken_ThrowsUnauthorizedException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var request = new RefreshTokenRequest("expired_access_token", "revoked_token");
        var storedToken = new TokensAtualizacao { UsuarioId = userId, Token = "revoked_token", RevogadoEm = DateTime.UtcNow };

        _jwtTokenService.GetUserIdFromExpiredToken(request.AccessToken).Returns(userId);
        _refreshTokenRepository.GetByTokenAsync(request.RefreshToken, Arg.Any<CancellationToken>()).Returns(storedToken);

        // Act
        Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedException>()
            .WithMessage("Refresh token foi revogado. Faça login novamente.");
    }
}
