using SolicitudesApp.Application.DTOs;
using SolicitudesApp.Application.Interfaces;
using SolicitudesApp.Application.Services;
using SolicitudesApp.Domain.Entidades;

namespace SolicitudesApp.Application.Tesoreria
{
    // Solo lectura: los movimientos se registran al pagar una solicitud (POST /api/solicitudes/{id}/pagar)
    public class MovimientoTesoreriaService : IMovimientoTesoreriaService
    {
        private readonly IUnitOfWork _uow;
        public MovimientoTesoreriaService(IUnitOfWork uow) => _uow = uow;

        public async Task<IReadOnlyList<MovimientoTesoreriaDto>> ListarAsync(CancellationToken ct = default) =>
            (await _uow.MovimientosTesoreria.ListAllAsync(ct)).OrderByDescending(m => m.FechaPago).Select(MapearADto).ToList();

        public async Task<MovimientoTesoreriaDto?> ObtenerPorIdAsync(int id, CancellationToken ct = default)
        {
            var movimiento = await _uow.MovimientosTesoreria.GetByIdAsync(id, ct);
            return movimiento is null ? null : MapearADto(movimiento);
        }

        private static MovimientoTesoreriaDto MapearADto(MovimientoTesoreria m) => new(
            m.Id, m.SolicitudId, $"SOL-{m.SolicitudId:D5}", m.MontoPagado, m.Medio, m.Referencia, m.FechaPago, m.CreadoPor);
    }
}
