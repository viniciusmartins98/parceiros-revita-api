namespace RevitaParceiros.Application.Common.Interfaces;

/// <summary>
/// Serviço responsável pelo hash e verificação de senhas.
/// </summary>
public interface IPasswordHasher
{
    /// <summary>
    /// Gera um hash seguro a partir de uma senha em texto claro.
    /// </summary>
    string Hash(string password);

    /// <summary>
    /// Verifica se uma senha em texto claro corresponde a um hash gerado.
    /// </summary>
    bool Verify(string password, string hash);
}
