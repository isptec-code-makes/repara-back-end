using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repara.Model;

namespace DAL.Configurations;

public class SolicitacaoConfig : IEntityTypeConfiguration<Solicitacao>
{
    public void Configure(EntityTypeBuilder<Solicitacao> builder)
    {
        builder
            .Property(c => c.Estado).HasColumnType("tinyint");

        builder
            .Property(c => c.Prioridade).HasColumnType("tinyint");

        builder
            .HasOne(d => d.Funcionario)
            .WithMany(f => f.Solicitacoes)
            .HasForeignKey(d => d.FuncionarioId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(p => p.Cliente)
            .WithMany(u => u.Solicitacoes)
            .HasForeignKey(p => p.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}