using SolicitudesApp.Application.DTOs;
using SolicitudesApp.Domain.Entidades;

namespace SolicitudesApp.Application.Interfaces
{
    public interface  ISolicitudRepository: IRepository<Solicitud>
    {
        Task<Solicitud?> GetByIdConDetallesAsync(int id, CancellationToken ct = default);

        Task<PagedResult<Solicitud>> ObtenerPaginadoAsync(int pagina, int tamanoPagina, EstadoSolicitud? estado, CancellationToken ct = default);
    }
}
