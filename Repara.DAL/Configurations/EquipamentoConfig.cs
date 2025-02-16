using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repara.Model;

namespace DAL.Configurations;

public class EquipamentoConfig : IEntityTypeConfiguration<Equipamento>
{
    public void Configure(EntityTypeBuilder<Equipamento> builder)
    {
        builder
            .Property(c => c.Categoria).HasColumnType("tinyint");

        builder
            .HasOne(p => p.Solicitacao)
            .WithMany(u => u.Equipamentos)
            .HasForeignKey(p => p.SolicitacaoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}