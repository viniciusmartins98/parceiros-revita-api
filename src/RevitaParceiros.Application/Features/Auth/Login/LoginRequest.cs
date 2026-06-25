using Mediator;

namespace RevitaParceiros.Application.Features.Auth.Login;

public sealed record LoginRequest(string Login, string Senha) : IRequest<LoginResponse>;
