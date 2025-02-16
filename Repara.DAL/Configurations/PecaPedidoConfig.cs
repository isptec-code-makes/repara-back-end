using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repara.Model;

namespace DAL.Configurations;

public class PecaPedidoConfig : IEntityTypeConfiguration<PecaPedido>
{
    public void Configure(EntityTypeBuilder<PecaPedido> builder)
    {
        builder
            .Property(c => c.Estado).HasColumnType("tinyint");

        builder
            .HasOne(p => p.Peca)
            .WithMany(u => u.PecaPedidos)
            .HasForeignKey(p => p.PecaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
           .HasOne(u => u.Montagem)
           .WithOne(p => p.PecaPedido)
           .HasForeignKey<PecaPedido>(p => p.MontagemId);
    }
}