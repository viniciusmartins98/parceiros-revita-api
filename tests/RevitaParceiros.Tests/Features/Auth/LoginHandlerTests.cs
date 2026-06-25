using FluentAssertions;
using NSubstitute;
using RevitaParceiros.Application.Common.Interfaces;
using RevitaParceiros.Application.Features.Auth.Login;
using RevitaParceiros.Domain.Entities;
using RevitaParceiros.Domain.Exceptions;
using RevitaParceiros.Domain.Interfaces;
using Xunit;

namespace RevitaParceiros.Tests.Features.Auth;

public class LoginHandlerTests
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly LoginHandler _handler;

    public LoginHandlerTests()
    {
        _usuarioRepository = Substitute.For<IUsuarioRepository>();
        _refreshTokenRepository = Substitute.For<IRefreshTokenRepository>();
        _jwtTokenService = Substitute.For<IJwtTokenService>();
        _passwordHasher = Substitute.For<IPasswordHasher>();
        _dateTimeProvider = Substitute.For<IDateTimeProvider>();
        
        _handler = new LoginHandler(
            _usuarioRepository, 
            _refreshTokenRepository, 
            _jwtTokenService, 
            _passwordHasher, 
            _dateTimeProvider);
    }

    [Fact]
    public async Task Login_WithValidEmailAndPassword_ReturnsTokens()
    {
        // Arrange
        var request = new LoginRequest("teste@teste.com", "senha123");
        var usuario = new Usuarios { Id = Guid.NewGuid(), Nome = "Teste", Email = request.Login, SenhaHash = "hash", Ativo = true };
        
        _usuarioRepository.GetByEmailAsync(request.Login, Arg.Any<CancellationToken>()).Returns(usuario);
        _passwordHasher.Verify(request.Senha, usuario.SenhaHash).Returns(true);
        _jwtTokenService.GenerateAccessToken(usuario.Id, usuario.Nome, Arg.Any<string>()).Returns("access_token");
        _jwtTokenService.GenerateRefreshToken().Returns("refresh_token");
        _dateTimeProvider.UtcNow.Returns(DateTime.UtcNow);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.AccessToken.Should().Be("access_token");
        result.RefreshToken.Should().Be("refresh_token");
        await _refreshTokenRepository.Received(1).AddAsync(Arg.Any<TokensAtualizacao>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Login_WithInvalidPassword_ThrowsUnauthorizedException()
    {
        // Arrange
        var request = new LoginRequest("teste@teste.com", "senha_errada");
        var usuario = new Usuarios { Id = Guid.NewGuid(), Nome = "Teste", Email = request.Login, SenhaHash = "hash", Ativo = true };
        
        _usuarioRepository.GetByEmailAsync(request.Login, Arg.Any<CancellationToken>()).Returns(usuario);
        _passwordHasher.Verify(request.Senha, usuario.SenhaHash).Returns(false);

        // Act
        Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedException>()
            .WithMessage("Login ou senha incorretos.");
    }
    
    [Fact]
    public async Task Login_WithInactiveUser_ThrowsUnauthorizedException()
    {
        // Arrange
        var request = new LoginRequest("teste@teste.com", "senha123");
        var usuario = new Usuarios { Id = Guid.NewGuid(), Nome = "Teste", Email = request.Login, SenhaHash = "hash", Ativo = false }; // Inativo
        
        _usuarioRepository.GetByEmailAsync(request.Login, Arg.Any<CancellationToken>()).Returns(usuario);
        _passwordHasher.Verify(request.Senha, usuario.SenhaHash).Returns(true);

        // Act
        Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedException>()
            .WithMessage("Usuário inativo. Entre em contato com o suporte.");
    }
}
