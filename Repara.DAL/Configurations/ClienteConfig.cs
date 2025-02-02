using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repara.Model;

namespace DAL.Configurations;

public class ClienteConfig : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder.HasOne(d => d.User)
               .WithOne()
               .HasForeignKey<Cliente>(d => d.UserId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}