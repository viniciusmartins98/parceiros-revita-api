using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RevitaParceiros.Domain.Entities;
using RevitaParceiros.Domain.Enums;

namespace RevitaParceiros.Infra.Persistence;

public partial class DatabaseContext : DbContext
{
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuarios>(entity =>
            entity.Property(p => p.Perfil)
                .HasConversion(new EnumToStringConverter<PerfilUsuarioEnum>())
        );

        modelBuilder.Entity<Resgates>(entity =>
            entity.Property(p => p.Status)
                .HasConversion(new EnumToStringConverter<StatusResgateEnum>())
        );

        modelBuilder.Entity<ExtratoPontos>(entity =>
            entity.Property(p => p.TipoTransacao)
                .HasConversion(new EnumToStringConverter<TipoTransacaoPontosEnum>())
        );

        modelBuilder.Entity<FaixasPontuacao>(entity =>
            entity.Property(p => p.Tipo)
                .HasConversion(new EnumToStringConverter<TipoFaixaPontuacaoEnum>())
        );
    }
}