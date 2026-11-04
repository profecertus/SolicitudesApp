using SolicitudesApp.Application.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SolicitudesApp.Application.Interfaces;
using SolicitudesApp.Application.Solicitudes;
using SolicitudesApp.Application.Aprobaciones;
using SolicitudesApp.Application.CentrosCosto;
using SolicitudesApp.Application.Roles;
using SolicitudesApp.Application.Tesoreria;
using SolicitudesApp.Application.Usuarios;
using SolicitudesApp.Infrastructure.Persistencia;
using SolicitudesApp.Infrastructure.Repositorios;

namespace SolicitudesApp.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<SolicitudesDbContext>(
                options => options.UseSqlServer(connectionString)
                );
            services.AddScoped<IUnitOfWork, UnitOfWork>(); //Acá estaba relacionando la interfaz con la clase
            services.AddScoped<ISolicitudRepository, SolicitudRepository>();//Acá estaba relacionando la interfaz con la clase
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<ISolicitudService, SolicitudService>();
            services.AddScoped<ISolicitudDetalleService, SolicitudDetalleService>();
            services.AddScoped<ICentroCostoService, CentroCostoService>();
            services.AddScoped<IRolService, RolService>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IAprobacionService, AprobacionService>();
            services.AddScoped<IMovimientoTesoreriaService, MovimientoTesoreriaService>();
            return services;
        }
    }
}
