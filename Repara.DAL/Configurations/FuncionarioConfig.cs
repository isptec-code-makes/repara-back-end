using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repara.Model;

namespace DAL.Configurations;

public class FuncionarioConfig : IEntityTypeConfiguration<Funcionario>
{
  public void Configure(EntityTypeBuilder<Funcionario> builder)
  {

    builder
      .HasOne(d => d.User)
      .WithOne()
      .HasForeignKey<Funcionario>(d => d.UserId)
      .OnDelete(DeleteBehavior.Restrict);
  }
}
