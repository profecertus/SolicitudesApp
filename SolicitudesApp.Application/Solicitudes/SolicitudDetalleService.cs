using FluentValidation;
using SolicitudesApp.Application.DTOs;
using SolicitudesApp.Application.Interfaces;
using SolicitudesApp.Application.Services;
using SolicitudesApp.Domain.Entidades;
using SolicitudesApp.Domain.Excepciones;

namespace SolicitudesApp.Application.Solicitudes
{
    // Los detalles pertenecen a la solicitud: solo se modifican en Borrador
    // y cada cambio recalcula el MontoTotal de la solicitud.
    public class SolicitudDetalleService : ISolicitudDetalleService
    {
        private readonly IUnitOfWork _uow;
        private readonly IValidator<GuardarDetalleDto> _validator;

        public SolicitudDetalleService(IUnitOfWork uow, IValidator<GuardarDetalleDto> validator)
        {
            _uow = uow;
            _validator = validator;
        }

        public async Task<IReadOnlyList<SolicitudDetalleDto>> ListarAsync(int solicitudId, CancellationToken ct = default)
        {
            var solicitud = await ObtenerSolicitudAsync(solicitudId, ct);
            return solicitud.Detalles.OrderBy(d => d.Id).Select(MapearADto).ToList();
        }

        public async Task<SolicitudDetalleDto?> ObtenerPorIdAsync(int solicitudId, int detalleId, CancellationToken ct = default)
        {
            var solicitud = await ObtenerSolicitudAsync(solicitudId, ct);
            var detalle = solicitud.Detalles.FirstOrDefault(d => d.Id == detalleId);
            return detalle is null ? null : MapearADto(detalle);
        }

        public async Task<SolicitudDetalleDto> CrearAsync(int solicitudId, GuardarDetalleDto dto, CancellationToken ct = default)
        {
            await _validator.ValidateAndThrowAsync(dto, ct);
            var solicitud = await ObtenerSolicitudEditableAsync(solicitudId, ct);

            var detalle = new SolicitudDetalle
            {
                Concepto = dto.Concepto.Trim(),
                Cantidad = dto.Cantidad,
                PrecioUnitario = dto.PrecioUnitario
            };
            solicitud.Detalles.Add(detalle);
            solicitud.RecalcularTotal();
            await _uow.SaveChangesAsync(ct);
            return MapearADto(detalle);
        }

        public async Task ActualizarAsync(int solicitudId, int detalleId, GuardarDetalleDto dto, CancellationToken ct = default)
        {
            await _validator.ValidateAndThrowAsync(dto, ct);
            var solicitud = await ObtenerSolicitudEditableAsync(solicitudId, ct);
            var detalle = ObtenerDetalle(solicitud, detalleId);

            detalle.Concepto = dto.Concepto.Trim();
            detalle.Cantidad = dto.Cantidad;
            detalle.PrecioUnitario = dto.PrecioUnitario;
            solicitud.RecalcularTotal();
            await _uow.SaveChangesAsync(ct);
        }

        public async Task EliminarAsync(int solicitudId, int detalleId, CancellationToken ct = default)
        {
            var solicitud = await ObtenerSolicitudEditableAsync(solicitudId, ct);
            var detalle = ObtenerDetalle(solicitud, detalleId);
            if (solicitud.Detalles.Count == 1)
                throw new ReglaDominioException("La solicitud debe tener al menos un item de detalle");

            solicitud.Detalles.Remove(detalle);
            await _uow.SolicitudDetalles.DeleteAsync(detalle, ct);
            solicitud.RecalcularTotal();
            await _uow.SaveChangesAsync(ct);
        }

        private async Task<Solicitud> ObtenerSolicitudAsync(int solicitudId, CancellationToken ct) =>
            await _uow.Solicitudes.GetByIdConDetallesAsync(solicitudId, ct)
                ?? throw new KeyNotFoundException($"No se encontró la solicitud con Id {solicitudId}");

        private async Task<Solicitud> ObtenerSolicitudEditableAsync(int solicitudId, CancellationToken ct)
        {
            var solicitud = await ObtenerSolicitudAsync(solicitudId, ct);
            if (solicitud.Estado != EstadoSolicitud.Borrador)
                throw new ReglaDominioException("Solo se pueden modificar los detalles de solicitudes en estado Borrador.");
            return solicitud;
        }

        private static SolicitudDetalle ObtenerDetalle(Solicitud solicitud, int detalleId) =>
            solicitud.Detalles.FirstOrDefault(d => d.Id == detalleId)
                ?? throw new KeyNotFoundException($"No se encontró el detalle {detalleId} en la solicitud {solicitud.Id}");

        private static SolicitudDetalleDto MapearADto(SolicitudDetalle d) =>
            new(d.Id, d.Concepto, d.Cantidad, d.PrecioUnitario, d.Importe);
    }
}
