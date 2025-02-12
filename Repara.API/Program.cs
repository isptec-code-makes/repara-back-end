using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Repara.API.Extensions;
using Repara.DAL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
});

builder.Services.AddProblemDetails();

//builder.Services.AddSqlServerDatabaseManager(builder.Configuration);

builder.Services.AddDbContext<AppDbContext>(options =>
{
    // Configura o uso do SQL Server com a string de conexão fornecida
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"), x => x.MigrationsAssembly("Repara.DAL"));
});

builder.Services.AddIdentityManager(builder.Configuration);
builder.Services.AddDependenciesManager();

builder.Services.AddCors(options =>
{

    options.AddPolicy("CorsPolicy", builder =>
        builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithExposedHeaders("X-Pagination")
            );
});

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

app.UseExceptionHandler(_=> {});
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
