namespace RevitaParceiros.Application.Common.Interfaces;

/// <summary>
/// Serviço responsável pela geração e validação de tokens JWT e Refresh Tokens.
/// </summary>
public interface IJwtTokenService
{
    /// <summary>
    /// Gera um token de acesso JWT.
    /// </summary>
    string GenerateAccessToken(Guid userId, string name, string role);

    /// <summary>
    /// Gera um token de atualização (refresh token) criptograficamente seguro.
    /// </summary>
    string GenerateRefreshToken();

    /// <summary>
    /// Extrai o ID do usuário de um token JWT expirado sem validar a expiração, 
    /// mas validando a assinatura e os outros parâmetros.
    /// </summary>
    Guid? GetUserIdFromExpiredToken(string token);
}
