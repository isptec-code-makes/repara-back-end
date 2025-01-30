using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repara.Model;

namespace DAL.Configurations;

public class FuncionarioConfig: IEntityTypeConfiguration<Funcionario>
{
    public void Configure(EntityTypeBuilder<Funcionario> builder)
    {
        throw new NotImplementedException();
    }
}