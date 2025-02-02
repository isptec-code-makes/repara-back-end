using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repara.Model;

namespace DAL.Configurations;

public class FuncionarioConfig : IEntityTypeConfiguration<Funcionario>
{
    public void Configure(EntityTypeBuilder<Funcionario> builder)
    {
        builder.HasMany(f => f.Diagnosticos)
            .WithOne(d => d.Funcionario)
            .HasForeignKey(d => d.FuncionarioId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(f => f.Montagens)
          .WithOne(d => d.Funcionario)
          .HasForeignKey(d => d.FuncionarioId)
          .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(f => f.Solicitacoes)
          .WithOne(d => d.Funcionario)
          .HasForeignKey(d => d.FuncionarioId)
          .OnDelete(DeleteBehavior.Restrict);

         builder.HasOne(d => d.User)
               .WithOne()
               .HasForeignKey<Funcionario>(d => d.UserId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
