using SolicitudesApp.Application.DTOs;

namespace SolicitudesApp.Application.Services
{
    public interface ICentroCostoService
    {
        Task<IReadOnlyList<CentroCostoDto>> ListarAsync(bool? activo = null, CancellationToken ct = default);
        Task<CentroCostoDto?> ObtenerPorIdAsync(int id, CancellationToken ct = default);
        Task<CentroCostoDto> CrearAsync(CrearCentroCostoDto dto, CancellationToken ct = default);
        Task ActualizarAsync(int id, ActualizarCentroCostoDto dto, CancellationToken ct = default);
        Task EliminarAsync(int id, CancellationToken ct = default);
    }

    public interface IRolService
    {
        Task<IReadOnlyList<RolDto>> ListarAsync(CancellationToken ct = default);
        Task<RolDto?> ObtenerPorIdAsync(int id, CancellationToken ct = default);
        Task<RolDto> CrearAsync(GuardarRolDto dto, CancellationToken ct = default);
        Task ActualizarAsync(int id, GuardarRolDto dto, CancellationToken ct = default);
        Task EliminarAsync(int id, CancellationToken ct = default);
    }

    public interface IUsuarioService
    {
        Task<IReadOnlyList<UsuarioDto>> ListarAsync(CancellationToken ct = default);
        Task<UsuarioDto?> ObtenerPorIdAsync(int id, CancellationToken ct = default);
        Task<UsuarioDto> CrearAsync(CrearUsuarioDto dto, CancellationToken ct = default);
        Task ActualizarAsync(int id, ActualizarUsuarioDto dto, CancellationToken ct = default);
        Task EliminarAsync(int id, CancellationToken ct = default);
        Task<IReadOnlyList<RolDto>> ListarRolesAsync(int usuarioId, CancellationToken ct = default);
        Task AsignarRolAsync(int usuarioId, int rolId, CancellationToken ct = default);
        Task QuitarRolAsync(int usuarioId, int rolId, CancellationToken ct = default);
    }

    public interface ISolicitudDetalleService
    {
        Task<IReadOnlyList<SolicitudDetalleDto>> ListarAsync(int solicitudId, CancellationToken ct = default);
        Task<SolicitudDetalleDto?> ObtenerPorIdAsync(int solicitudId, int detalleId, CancellationToken ct = default);
        Task<SolicitudDetalleDto> CrearAsync(int solicitudId, GuardarDetalleDto dto, CancellationToken ct = default);
        Task ActualizarAsync(int solicitudId, int detalleId, GuardarDetalleDto dto, CancellationToken ct = default);
        Task EliminarAsync(int solicitudId, int detalleId, CancellationToken ct = default);
    }

    public interface IAprobacionService
    {
        Task<IReadOnlyList<AprobacionDto>> ListarAsync(int? solicitudId = null, CancellationToken ct = default);
        Task<AprobacionDto?> ObtenerPorIdAsync(int id, CancellationToken ct = default);
    }

    public interface IMovimientoTesoreriaService
    {
        Task<IReadOnlyList<MovimientoTesoreriaDto>> ListarAsync(CancellationToken ct = default);
        Task<MovimientoTesoreriaDto?> ObtenerPorIdAsync(int id, CancellationToken ct = default);
    }
}
