using FluentValidation;
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

builder.Services.AddCors(options => {
    options.AddPolicy(CorsPolicy, policy =>
    {
        policy.WithOrigins("http://localhost:4200", "https://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Server=localhost;Database=SolicitudesDb;Trusted_Connection=True;TrustServerCertificate=True";

// Registro de Infrastructure: DbContext, Unit of Work, Repositorios y Servicios
builder.Services.AddInfrastructureServices(connectionString);
var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Inicialización de la base de datos de desarrollo
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SolicitudesDbContext>();
    db.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(CorsPolicy);
app.UseAuthorization();
app.MapControllers();
app.Run();