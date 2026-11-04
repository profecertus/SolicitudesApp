using SolicitudesApp.Domain.Entidades;

namespace SolicitudesApp.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ISolicitudRepository Solicitudes { get; }
        IRepository<CentroCosto> CentrosCosto {  get; }

        IUsuarioRepository Usuarios { get; }
        IAprobacionRepository Aprobaciones { get; }
        IRepository<Rol> Roles { get; }
        IRepository<UsuarioRol> UsuarioRoles { get; }
        IRepository<SolicitudDetalle> SolicitudDetalles { get; }
        IRepository<MovimientoTesoreria> MovimientosTesoreria { get; }
        Task<int> SaveChangesAsync(CancellationToken ct = default);

    }
}
