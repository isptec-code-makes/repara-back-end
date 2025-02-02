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
    }
}