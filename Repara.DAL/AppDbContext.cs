using DAL.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Repara.DAL.Configurations;
using Repara.Model;

namespace Repara.DAL;

public class AppDbContext : IdentityDbContext<User>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


    public DbSet<Cliente> Cliente { get; set; }
    public DbSet<Diagnostico> Diagnostico { get; set; }
    public DbSet<Equipamento> Equipamento { get; set; }
    public DbSet<Funcionario> Funcionario { get; set; }
    public DbSet<Montagem> Montagem { get; set; }
    public DbSet<Peca> Peca { get; set; }
    public DbSet<PecaPedido> PecaPedido { get; set; }
    public DbSet<Solicitacao> Solicitacao { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ClienteConfig());
        modelBuilder.ApplyConfiguration(new DiagnosticoConfig());
        modelBuilder.ApplyConfiguration(new EquipamentoConfig());
        modelBuilder.ApplyConfiguration(new FuncionarioConfig());
        modelBuilder.ApplyConfiguration(new MontagemConfig());
        modelBuilder.ApplyConfiguration(new PecaConfig());
        modelBuilder.ApplyConfiguration(new PecaPedidoConfig());
        modelBuilder.ApplyConfiguration(new SolicitacaoConfig());
        modelBuilder.ApplyConfiguration(new RoleConfig());
        modelBuilder.ApplyConfiguration(new UserConfig());

        base.OnModelCreating(modelBuilder);
    }
}