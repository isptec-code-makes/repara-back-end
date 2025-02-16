using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repara.Model;

namespace DAL.Configurations;

public class MontagemConfig : IEntityTypeConfiguration<Montagem>
{
    public void Configure(EntityTypeBuilder<Montagem> builder)
    {
        builder
            .Property(c => c.Estado).HasColumnType("tinyint");

        builder.HasOne(d => d.Funcionario)
            .WithMany(f => f.Montagens)
            .HasForeignKey(d => d.FuncionarioId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(p => p.Equipamento)
            .WithMany(u => u.Montagens)
            .HasForeignKey(p => p.EquipamentoId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}