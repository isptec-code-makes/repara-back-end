using System.Text;
using DAL.Repositories.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Repara.DAL;
using Repara.DAL.Repositories;
using Repara.DTO.Auth;
using Repara.Helpers;
using Repara.Helpers.Mappers;
using Repara.Model;
using Repara.Services;
using Repara.Services.Contracts;

namespace Repara.API.Extensions;

/// <summary>
/// Classe estática responsável por gerenciar as dependências e serviços da aplicação.
/// </summary>
public static class DependenciesManager
{
    /// <summary>
    /// Configura o Identity e a autenticação JWT para o aplicativo.
    /// </summary>
    /// <param name="services">A coleção de serviços da aplicação.</param>
    /// <param name="configuration">As configurações do aplicativo, incluindo o JWT.</param>
    /// <returns>Retorna a coleção de serviços configurada.</returns>
    public static IServiceCollection AddIdentityManager(this IServiceCollection services, IConfiguration configuration)
    {


        services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 8;
                options.User.AllowedUserNameCharacters = null;
                options.User.RequireUniqueEmail = false;
            })
            .AddRoles<IdentityRole>()
            .AddRoleManager<RoleManager<IdentityRole>>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
        
        services.Configure<JwtConfiguration>(
            configuration.GetSection(JwtConfiguration.Position));
        
        services.AddAuthentication(options =>
            { 
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = configuration["JWT:ValidAudience"],
                    ValidIssuer = configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"])),
                    ClockSkew = TimeSpan.Zero
                };
                
            });

            services.AddScoped<JwtHandler>();

            services.Configure<SocialLoginConfiguration>(configuration.GetSection(SocialLoginConfiguration.Position));


        return services;
    }

    /// <summary>
    /// Registra as dependências do repositório, serviços e outros componentes necessários na aplicação.
    /// </summary>
    /// <param name="services">A coleção de serviços da aplicação.</param>
    /// <returns>Retorna a coleção de serviços configurada.</returns>
    public static IServiceCollection AddDependenciesManager(this IServiceCollection services)
    {
        // Registra os repositórios e serviços relacionados à aplicação.
        services.AddScoped<IClienteRepository, ClienteRepository>();
        services.AddScoped<IDiagnosticoRepository, DiagnosticoRepository>();
        services.AddScoped<IEquipamentoRepository, EquipamentoRepository>();
        services.AddScoped<IFuncionarioRepository, FuncionarioRepository>();
        services.AddScoped<IMontagemRepository, MontagemRepository>();
        services.AddScoped<IPecaPedidoRepository, PecaPedidoRepository>();
        services.AddScoped<IPecaRepository, PecaRepository>();
        services.AddScoped<ISolicitacaoRepository, SolicitacaoRepository>();
        services.AddScoped<IClienteService, ClienteService>();
        services.AddScoped<IAuthService, AuthService>();

        
        


        services.AddAutoMapper(typeof(ClienteProfile));

        return services;
    }

    /// <summary>
    /// Configura os serviços externos, como e-mail, SMS e ProxyPay.
    /// </summary>
    /// <param name="services">A coleção de serviços da aplicação.</param>
    /// <param name="configuration">As configurações da aplicação.</param>
    /// <returns>Retorna a coleção de serviços configurada.</returns>
    public static IServiceCollection AddSExternalervices(this IServiceCollection services, IConfiguration configuration)
    {

        return services;
    }
}
