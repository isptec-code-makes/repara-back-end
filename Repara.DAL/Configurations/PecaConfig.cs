using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repara.Model;

namespace DAL.Configurations;

public class PecaConfig: IEntityTypeConfiguration<Peca>
{
    public void Configure(EntityTypeBuilder<Peca> builder)
    {
        builder
            .Property(c => c.Categoria).HasColumnType("tinyint");
    }
}