using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SolicitudesApp.Api.Middleware;
using SolicitudesApp.Application.validaciones;
using SolicitudesApp.Infrastructure;
using SolicitudesApp.Infrastructure.Persistencia;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddValidatorsFromAssemblyContaining<CrearSolicitudDtoValidator>();

const string CorsPolicy = "AllowAngularApp";

builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicy, policy =>
    {
        policy.WithOrigins("http://localhost:4200", "https://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var jwtKey = builder.Configuration["Jwt:Key"]
    ?? "ClaveSecretaSuperSeguraParaFirmarTokensSolicitudesApp2026FullStack!";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "SolicitudesApp.Api";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "SolicitudesApp.Client";

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ClockSkew = TimeSpan.Zero // Respeta estrictamente el TTL configurado en el JWT
        };
    });

builder.Services.AddAuthorization();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Server=localhost;Database=SolicitudesDb;Trusted_Connection=True;TrustServerCertificate=True";

// Registro de Infrastructure: DbContext, Unit of Work, Repositorios y Servicios (incluyendo IJwtService e IAuthService)
builder.Services.AddInfrastructureServices(connectionString);
var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Inicialización de la base de datos de desarrollo y semillas de usuarios con credenciales
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SolicitudesDbContext>();
    await DbInitializer.InicializarUsuariosSemillaAsync(db);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(CorsPolicy);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
