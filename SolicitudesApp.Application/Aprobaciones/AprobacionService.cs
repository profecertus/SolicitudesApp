using SolicitudesApp.Application.DTOs;
using SolicitudesApp.Application.Interfaces;
using SolicitudesApp.Application.Services;
using SolicitudesApp.Domain.Entidades;

namespace SolicitudesApp.Application.Aprobaciones
{
    // Solo lectura: las aprobaciones se registran desde las transiciones de estado de la solicitud
    public class AprobacionService : IAprobacionService
    {
        private readonly IUnitOfWork _uow;
        public AprobacionService(IUnitOfWork uow) => _uow = uow;

        public async Task<IReadOnlyList<AprobacionDto>> ListarAsync(int? solicitudId = null, CancellationToken ct = default) =>
            (await _uow.Aprobaciones.ListarConResponsableAsync(solicitudId, ct)).Select(MapearADto).ToList();

        public async Task<AprobacionDto?> ObtenerPorIdAsync(int id, CancellationToken ct = default)
        {
            var aprobacion = await _uow.Aprobaciones.GetByIdConResponsableAsync(id, ct);
            return aprobacion is null ? null : MapearADto(aprobacion);
        }

        private static AprobacionDto MapearADto(Aprobacion a) => new(
            a.Id, a.SolicitudId, $"SOL-{a.SolicitudId:D5}", a.EstadoResultante, a.EstadoResultante.ToString(),
            a.ResponsableId, a.Responsable?.Nombre ?? $"Usuario #{a.ResponsableId}", a.Comentario, a.Fecha);
    }
}
