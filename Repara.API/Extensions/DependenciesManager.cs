using System.Text;
using DAL;
using DAL.Repositories;
using DAL.Repositories.Contracts;
using Helpers.JwtFeatures;
using Helpers.Mappers;
using Importador;
using Message.Contracts;
using Message.Email;
using Message.SMS;
using Message.Whatsapp;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Model;
using PaymentGateway;
using PaymentGateway.Contracts;
using PaymentGateway.ProxyPay;
using PaymentGateway.ProxyPay.Contracts;
using Services;
using Services.Contracts;
using Shared.Constants;

namespace API.Extensions;

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
        // Configura o Identity com as políticas de senha e usuário.
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
            .AddEntityFrameworkStores<AppDbContext>();

        var jwtSettings = configuration.GetSection("JwtSettings");
        
        // Configura o esquema de autenticação JWT.
        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(opt =>
        {
            // Configura os parâmetros de validação do token JWT.
            opt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["validIssuer"],
                ValidAudience = jwtSettings["validAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                    .GetBytes(jwtSettings.GetSection("securityKey").Value))
            };
        });
        
        // Adiciona o manipulador de JWT à coleção de serviços.
        services.AddScoped<JwtHandler>();

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
        services.AddScoped<IPessoaRepository, PessoaRepository>();
        services.AddScoped<IPessoaService, PessoaService>();
        services.AddScoped<IPessoaPreferenciaRepository, PessoaPreferenciaRepository>();

        services.AddScoped<ICategoriaRepository, CategoriaRepository>();
        services.AddScoped<ICategoriaService, CategoriaService>();

        services.AddScoped<IAgregadoFamiliarRepository, AgregadoFamiliarRepository>();
        services.AddScoped<IAgregadoFamiliarService, AgregadoFamiliarService>();

        services.AddScoped<IInscricaoRepository, InscricaoRepository>();
        services.AddScoped<IInscricaoService, InscricaoService>();

        services.AddScoped<IPagamentoRepository, PagamentoRepository>();
        services.AddScoped<IPagamentoService, PagamentoService>();
        
        services.AddScoped<ICobrancaRepository, CobrancaRepository>();
        services.AddScoped<ICobrancaService, CobrancaService>();

        services.AddScoped<IQuestionarioRepository, QuestionarioRepository>();
        services.AddScoped<IQuestionarioService, QuestionarioService>();

        services.AddScoped<IQuestionarioRespostaRepository, QuestionarioRespostaRepository>();
        services.AddScoped<IQuestionarioRespostaService, QuestionarioRespostaService>();

        services.AddScoped<IManagerService, ManagerService>();

        services.AddScoped<IContactoGrupoRepository, ContactoGrupoRepository>();
        services.AddScoped<IContactoRepository, ContactoRepository>();
        services.AddScoped<IContactoGrupoRelacaoRepository, ContactoGrupoRelacaoRepository>();
        services.AddScoped<IContactoService, ContactoService>();
        services.AddScoped<IContactoGrupoService, ContactoGrupoService>();

        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<INotificationService, NotificationService>();

        services.AddScoped<IClubeRepository, ClubeRepository>();
        services.AddScoped<IClubeService, ClubeService>();
        services.AddScoped<IClubeBancoRepository, ClubeBancoRepository>();

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IHelpersService, HelpersService>();

        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ISmsService, SmsService>();
        services.AddScoped<IWhatsappService, WhatsappService>();
        services.AddScoped<IProxyPayService, ProxyPayService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<ImportadorPessoa>();
        services.AddScoped<ImportadorQuestionario>();

        // Registra dependências como singletons onde apropriado.
        services.AddSingleton<FileExtensions>();

        // Registra os mapeadores do AutoMapper.
        services.AddAutoMapper(typeof(AuthProfile));
        services.AddAutoMapper(typeof(ManagerProfile));
        services.AddAutoMapper(typeof(ClubeProfile));
        services.AddAutoMapper(typeof(PessoaProfile));
        services.AddAutoMapper(typeof(ContactoProfile));
        services.AddAutoMapper(typeof(QuestionarioProfile));
        services.AddAutoMapper(typeof(InscricaoProfile));
        services.AddAutoMapper(typeof(CategoriaProfile));
        services.AddAutoMapper(typeof(PagamentoProfile));
        services.AddAutoMapper(typeof(AgregadoFamiliarProfile));
        services.AddAutoMapper(typeof(MensagemProfile));

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
        // Carrega as configurações de serviços externos.
        var emailConfig = configuration
            .GetSection("EmailConfiguration")
            .Get<EmailConfiguration>();

        var smsconfig = configuration
            .GetSection("TwilioConfiguration")
            .Get<SmsConfiguration>();

        var proxyPayConfig = configuration
            .GetSection("ProxyPaySettings")
            .Get<ProxyPayConfiguration>();

        // Registra as configurações como singletons.
        services.AddSingleton(emailConfig);
        services.AddSingleton(smsconfig);
        services.AddSingleton(proxyPayConfig);

        return services;
    }
}
