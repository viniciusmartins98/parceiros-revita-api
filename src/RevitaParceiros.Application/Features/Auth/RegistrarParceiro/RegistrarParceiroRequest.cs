using Mediator;

namespace RevitaParceiros.Application.Features.Auth.RegistrarParceiro;

public sealed record RegistrarParceiroRequest(
    string Nome,
    string Email,
    string Telefone,
    string Senha) : IRequest<RegistrarParceiroResponse>;

public sealed record RegistrarParceiroResponse();
