using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repara.Model;

namespace DAL.Configurations;

public class MontagemConfig: IEntityTypeConfiguration<Montagem>
{
    public void Configure(EntityTypeBuilder<Montagem> builder)
    {
        builder
            .Property(c => c.Estado).HasColumnType("tinyint");
    }
}