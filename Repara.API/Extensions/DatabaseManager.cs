using DAL;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

/// <summary>
/// Classe de extensão para gerenciar configurações de banco de dados e migrações no aplicativo.
/// </summary>
public static class DatabaseManager
{
    /// <summary>
    /// Adiciona o gerenciador de banco de dados SQL Server ao serviço da aplicação.
    /// </summary>
    /// <param name="services">Coleção de serviços da aplicação.</param>
    /// <param name="configuration">Objeto de configuração contendo a string de conexão.</param>
    /// <returns>Retorna a coleção de serviços atualizada.</returns>
    public static IServiceCollection AddSqlServerDatabaseManager(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            // Configura o uso do SQL Server com a string de conexão fornecida
            options.UseSqlServer(configuration.GetConnectionString("SqlServer"));

            // Define o assembly para onde as migrações serão direcionadas
            //options.UseSqlServer(b => b.MigrationsAssembly("API"));
        });
        
        return services;
    }

    /// <summary>
    /// Adiciona o gerenciador de banco de dados SQLite ao serviço da aplicação.
    /// </summary>
    /// <param name="services">Coleção de serviços da aplicação.</param>
    /// <param name="configuration">Objeto de configuração contendo a string de conexão.</param>
    /// <returns>Retorna a coleção de serviços atualizada.</returns>
    public static IServiceCollection AddSqlLiteDatabaseManager(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            // Configura o uso do SQLite com a string de conexão fornecida
            options.UseSqlite(configuration.GetConnectionString("SqlLiteDb"));

            // Define o assembly para onde as migrações serão direcionadas
            //options.UseSqlite(b => b.MigrationsAssembly("API"));
        });
        
        return services;
    }

    /// <summary>
    /// Aplica automaticamente as migrações pendentes ao banco de dados configurado.
    /// </summary>
    /// <param name="webApp">Instância do aplicativo da web.</param>
    /// <returns>Retorna a instância atualizada do aplicativo da web.</returns>
    /// <exception cref="Exception">Lança uma exceção se ocorrer um erro ao aplicar as migrações.</exception>
    public static WebApplication MigrateDatabase(this WebApplication webApp)
    {
        using (var scope = webApp.Services.CreateScope())
        {
            using (var appContext = scope.ServiceProvider.GetRequiredService<AppDbContext>())
            {
                try
                {
                    // Aplica as migrações ao banco de dados
                    appContext.Database.Migrate();
                }
                catch (Exception ex)
                {
                    // Loga a exceção no console e re-lança a exceção
                    Console.WriteLine(ex);
                    // TODO: Logar no serilog . danger
                    throw;
                }
            }
        }
        return webApp;
    }
}
