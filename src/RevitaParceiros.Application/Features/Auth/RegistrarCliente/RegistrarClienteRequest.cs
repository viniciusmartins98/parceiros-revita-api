using Mediator;

namespace RevitaParceiros.Application.Features.Auth.RegistrarCliente;

public sealed record RegistrarClienteRequest(
    string Nome,
    string Email,
    string Telefone,
    string Cpf,
    string Senha) : IRequest<RegistrarClienteResponse>;

public sealed record RegistrarClienteResponse();
