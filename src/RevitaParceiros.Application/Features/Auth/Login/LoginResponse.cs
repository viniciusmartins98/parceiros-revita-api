namespace RevitaParceiros.Application.Features.Auth.Login;

public sealed record LoginResponse(
    string AccessToken,
    string RefreshToken,
    DateTime ExpiresAt,
    UserInfo User);

public sealed record UserInfo(
    Guid Id,
    Guid? EmployeeId,
    Guid? PartnerId,
    Guid? ClientId,
    string Nome,
    string Perfil);
