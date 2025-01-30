using DAL.Configurations;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

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
        
        base.OnModelCreating(modelBuilder);
    }
}