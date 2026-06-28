using Mediator;
using RevitaParceiros.Application.Features.Auth.Login;

namespace RevitaParceiros.Application.Features.Auth.RefreshToken;

public sealed record RefreshTokenRequest(
    string AccessToken,
    string RefreshToken) : IRequest<LoginResponse>;
