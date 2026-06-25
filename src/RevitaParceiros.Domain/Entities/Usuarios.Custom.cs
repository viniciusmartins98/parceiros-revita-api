using RevitaParceiros.Domain.Enums;

namespace RevitaParceiros.Domain.Entities;

/// <summary>
/// Tabela unificada de usuários do sistema (Administrador, Funcionario, Parceiro, Cliente).
/// </summary>
public partial class Usuarios
{
    public PerfilUsuarioEnum Perfil { get; set; }
}