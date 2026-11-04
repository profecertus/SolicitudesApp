using SolicitudesApp.Application.DTOs;
using SolicitudesApp.Domain.Entidades;

namespace SolicitudesApp.Application.Services
{
    public interface ISolicitudService
    {
        Task<SolicitudDto> CrearAsync(CrearSolicitudDto dto, CancellationToken ct = default);
        Task<SolicitudDto?> ObtenerPorIdAsync(int id, CancellationToken ct = default);
        Task<PagedResult<SolicitudDto>> ListarPaginadoAsync(int pagina, int tamanoPagina, EstadoSolicitud? estado = null, CancellationToken ct = default);
        Task ActualizarAsync(int id, ActualizarSolicitudDto dto, CancellationToken ct = default);
        Task EliminarAsync(int id, CancellationToken ct = default);
        Task<IReadOnlyList<ResumenEstadosDto>> ObtenerResumenPorEstadosAsync(CancellationToken ct = default);
        Task<ResultadoTransicionDto> EnviarAsync(int solicitudId, CancellationToken ct = default);
        Task<ResultadoTransicionDto> RevisarAsync(int solicitudId, int revisorId, CancellationToken ct = default);
        Task<ResultadoTransicionDto> ObservarAsync(int solicitudId, ObservarSolicitudDto dto, CancellationToken ct = default);
        Task<ResultadoTransicionDto> AprobarAsync(int solicitudId, AprobarSolicitudDto dto, CancellationToken ct = default);
        Task<ResultadoTransicionDto> RechazarAsync(int solicitudId, RechazarSolicitudDto dto, CancellationToken ct = default);
        Task<ResultadoTransicionDto> PagarAsync(int solicitudId, PagarSolicitudDto dto, CancellationToken ct = default);

        Task<IReadOnlyList<TimelineItemDto>> ObtenerTimelineAsync(int id, CancellationToken ct = default);
    }
}
