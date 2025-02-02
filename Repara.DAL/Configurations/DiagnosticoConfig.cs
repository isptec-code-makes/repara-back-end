using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repara.Model;

namespace DAL.Configurations;

public class DiagnosticoConfig : IEntityTypeConfiguration<Diagnostico>
{
    public void Configure(EntityTypeBuilder<Diagnostico> builder)
    {
        builder
           .Property(c => c.Estado).HasColumnType("tinyint");

        builder.HasOne(d => d.Funcionario)
            .WithMany(f => f.Diagnosticos)
            .HasForeignKey(d => d.FuncionarioId)
            .OnDelete(DeleteBehavior.Restrict);

        // Outras configurações...
    }
}
