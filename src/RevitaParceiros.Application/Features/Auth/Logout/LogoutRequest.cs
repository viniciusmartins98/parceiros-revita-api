using Mediator;

namespace RevitaParceiros.Application.Features.Auth.Logout;

public sealed record LogoutRequest(string RefreshToken) : IRequest<Unit>;
