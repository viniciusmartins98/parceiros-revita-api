namespace RevitaParceiros.Application.Common.Interfaces;

/// <summary>
/// Serviço responsável pela geração e validação de tokens JWT e Refresh Tokens.
/// </summary>
public interface IJwtTokenService
{
    /// <summary>
    /// Gera um token de acesso JWT com claims específicos de perfis.
    /// </summary>
    string GenerateAccessToken(Guid userId, string name, string role, Guid? employeeId = null, Guid? partnerId = null, Guid? clientId = null);

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
