namespace RevitaParceiros.Domain.Enums;

/// <summary>
/// Perfis de acesso do sistema Revita.
/// </summary>
public enum PerfilUsuarioEnum
{
    /// <summary>
    /// Administrador do sistema — acesso total.
    /// </summary>
    Administrador,

    /// <summary>
    /// Funcionário da loja — operacional (lançamento de compras, cadastro de clientes/parceiros).
    /// </summary>
    Funcionario,

    /// <summary>
    /// Parceiro — acompanhamento de clientes, saldo de pontos e extrato.
    /// </summary>
    Parceiro,

    /// <summary>
    /// Cliente (consumidor final) — acesso ao próprio extrato de compras.
    /// </summary>
    Cliente
}
