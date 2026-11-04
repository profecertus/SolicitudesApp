using SolicitudesApp.Application.DTOs;
using SolicitudesApp.Application.Interfaces;
using SolicitudesApp.Domain.Entidades;
using SolicitudesApp.Domain.Excepciones;
using SolicitudesApp.Application.Services;
using SolicitudesApp.Application.Aprobaciones;
using System.ComponentModel;


namespace SolicitudesApp.Application.Solicitudes
{
    public class SolicitudService : ISolicitudService
    {
        private readonly IUnitOfWork _uow;
        public SolicitudService(IUnitOfWork uow)
        {
            _uow = uow;            
        }

        public async Task<SolicitudDto> CrearAsync(CrearSolicitudDto dto, CancellationToken ct = default)
        {
            if (dto.Detalles == null || !dto.Detalles.Any())
                throw new ReglaDominioException("La solicitud debe tener al menos un item de detalle");

            var solicitud = new Solicitud
            {
                Motivo = dto.Motivo,
                SolicitanteId = dto.SolicitanteId,
                CentroCostoId = dto.CentroCostoId
            };

            foreach (var d in dto.Detalles)
                solicitud.Detalles.Add(new SolicitudDetalle
                {
                    Concepto = d.Concepto,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario
                });

            solicitud.RecalcularTotal();
            await _uow.Solicitudes.AddAsync(solicitud, ct);
            await _uow.SaveChangesAsync(ct);
            var recargada = await _uow.Solicitudes.GetByIdConDetallesAsync(solicitud.Id, ct);
            return MapearADto(recargada!);
        }

        public async Task<SolicitudDto?> ObtenerPorIdAsync(int id, CancellationToken ct = default)
        {
            var s = await _uow.Solicitudes.GetByIdConDetallesAsync(id, ct);
            return s is null ? null : MapearADto(s);
        }

        public async Task<PagedResult<SolicitudDto>> ListarPaginadoAsync(int pagina, int tamanoPagina, EstadoSolicitud? estado, CancellationToken ct = default)
        {
            var pagedEntities = await _uow.Solicitudes.ObtenerPaginadoAsync(pagina, tamanoPagina, estado, ct);
            var dtos = pagedEntities.Items.Select(MapearADto).ToList();
            return new PagedResult<SolicitudDto>(dtos, pagedEntities.TotalPaginas, pagedEntities.PaginaActual, pagedEntities.TamanoPagina, pagedEntities.TotalPaginas);
        }

        public async Task ActualizarAsync(int id, ActualizarSolicitudDto dto, CancellationToken ct = default)
        {
            var solicitud = await _uow.Solicitudes.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"No se encontró la solicitud con Id {id}");
            if (solicitud.Estado != EstadoSolicitud.Borrador)
                throw new ReglaDominioException("Solo se pueden editar solicitudes en estado Borrador.");
            solicitud.Motivo = dto.Motivo;
            solicitud.CentroCostoId = dto.CentroCostoId;
            await _uow.Solicitudes.UpdateAsync(solicitud, ct);
            await _uow.SaveChangesAsync(ct);
        }

        public async Task EliminarAsync(int id, CancellationToken ct = default)
        {
            var solicitud = await _uow.Solicitudes.GetByIdAsync(id, ct)
                ?? throw new KeyNotFoundException($"No se encontró la solicitud con Id {id}");
            if (solicitud.Estado != EstadoSolicitud.Borrador)
                throw new ReglaDominioException("Solo se pueden eliminar solicitudes en estado Borrador.");
            await _uow.Solicitudes.DeleteAsync(solicitud, ct);
            await _uow.SaveChangesAsync(ct);
        }
        public async Task<IReadOnlyList<ResumenEstadosDto>> ObtenerResumenPorEstadosAsync(CancellationToken ct = default)
        {
            var todas = await _uow.Solicitudes.ListAllAsync(ct);
            return todas
            .GroupBy(s => s.Estado)
            .Select(g => new ResumenEstadosDto(g.Key.ToString(), g.Count(), g.Sum(s => s.MontoTotal)))
            .ToList();
        }

        private static SolicitudDto MapearADto(Solicitud s) => new(
            s.Id, $"SOL-{s.Id:D5}", s.Motivo, s.MontoTotal, s.Estado, s.Estado.ToString(),
            s.SolicitanteId, s.Solicitante?.Nombre ?? "N/A",
            s.CentroCostoId, s.CentroCosto?.Nombre ?? "N/A", s.FechaCreacion,
            s.Detalles.Select(d => new SolicitudDetalleDto(d.Id, d.Concepto, d.Cantidad, d.PrecioUnitario, d.Importe)).ToList()
            );

        public async Task<ResultadoTransicionDto> EnviarAsync(int solicitudId, CancellationToken ct = default)
        {
            var solicitud = await _uow.Solicitudes.GetByIdConDetallesAsync(solicitudId);
            if (solicitud is null)
                throw new KeyNotFoundException($"No se encontró la solicitud con ID {solicitudId}");

            var usuario = await _uow.Usuarios.GetByIdAsync(solicitud.SolicitanteId, ct)
                ?? throw new KeyNotFoundException($"Solicitante {solicitud.SolicitanteId} no encontrado");

            solicitud.CambiarEstado(EstadoSolicitud.Enviada, usuario, "Envio de solicitud" );

            await _uow.SaveChangesAsync(ct);

            return new ResultadoTransicionDto(
                solicitud.Id,
                solicitud.Estado.ToString(),
                "La Solicitud fue enviada exitosamente"
                );
        }

        public async Task<ResultadoTransicionDto> RevisarAsync(int solicitudId, int revisorId, CancellationToken ct = default)
        {
            //Obtengo la solicitud
            var solicitud = await _uow.Solicitudes.GetByIdConDetallesAsync(solicitudId, ct);
            //Valido que la solicitud haya cargado
            if (solicitud is null)
                throw new KeyNotFoundException($"No se encontró la solicitud con ID {solicitudId}");

            var revisor = await _uow.Usuarios.GetByIdAsync(revisorId, ct)
                ?? throw new KeyNotFoundException($"Revisor con Id {revisorId} no existe");

            solicitud.CambiarEstado(EstadoSolicitud.EnRevision, revisor, "Revision iniciada");
            await _uow.SaveChangesAsync(ct);
            return new ResultadoTransicionDto(
                solicitud.Id,
                solicitud.Estado.ToString(),
                "La Solicitud fue enviada a revision exitosamente"
                );
        }


        public async Task<ResultadoTransicionDto> ObservarAsync(int solicitudId, ObservarSolicitudDto dto, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(dto.Observaciones))
                throw new ReglaDominioException("Debe indicar las observaciones para devolver la solicitud a borrador");

            var solicitud = await _uow.Solicitudes.GetByIdConDetallesAsync(solicitudId, ct)
                ?? throw new KeyNotFoundException($"No se encontro la solicitud {solicitudId}");

            var revisor = await _uow.Usuarios.GetByIdAsync(dto.ResponsableId, ct)
                ?? throw new KeyNotFoundException($"Revisor con Id {dto.ResponsableId} no existe.");

            solicitud.CambiarEstado(EstadoSolicitud.Borrador, revisor, dto.Observaciones);
            await _uow.SaveChangesAsync(ct);
            return new ResultadoTransicionDto(
                solicitud.Id,
                solicitud.Estado.ToString(),
                "La Solicitud fue enviada a borrador exitosamente (Observada)"
                );
        }

        public async Task<ResultadoTransicionDto> AprobarAsync(int solicitudId, AprobarSolicitudDto dto, CancellationToken ct = default)
        {
            var solicitud = await _uow.Solicitudes.GetByIdConDetallesAsync(solicitudId, ct)
                ?? throw new KeyNotFoundException($"No se encontró la solicitud {solicitudId}");

            var aprobador = await _uow.Usuarios.GetByIdAsync(dto.ResponsableId, ct)
                ?? throw new KeyNotFoundException($"Usuario aprobador con Id {dto.ResponsableId} no existe");

            var nivelRequerido = AsignadorAprobador.Determinar(solicitud.MontoTotal);

            solicitud.CambiarEstado(EstadoSolicitud.Aprobada, aprobador, dto.Observaciones ?? $"Aprobada por nivel {nivelRequerido}");

            await _uow.SaveChangesAsync(ct);
            return new ResultadoTransicionDto(
                solicitud.Id,
                solicitud.Estado.ToString(),
                "La Solicitud fue aprobada exitosamente"
                );
        }

        public async Task<ResultadoTransicionDto> RechazarAsync(int solicitudId, RechazarSolicitudDto dto, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(dto.MotivoRechazo))
                throw new ReglaDominioException("El motivo de rechazo es obligatorio para auditar la decisión");

            var solicitud = await _uow.Solicitudes.GetByIdAsync(solicitudId, ct)
                ?? throw new KeyNotFoundException($"No se encontró la soliciutd {solicitudId}");

            var responsable = await _uow.Usuarios.GetByIdAsync(dto.ResponsableId, ct)
                ?? throw new KeyNotFoundException($"Responsable {dto.ResponsableId} no encontrado");

            solicitud.CambiarEstado(EstadoSolicitud.Rechazada, responsable, dto.MotivoRechazo);
            await _uow.SaveChangesAsync(ct);
            return new ResultadoTransicionDto(
                solicitud.Id,
                solicitud.Estado.ToString(),
                "La Solicitud fue Rechazada"
                );
        }

        public async Task<ResultadoTransicionDto> PagarAsync(int solicitudId, PagarSolicitudDto dto, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(dto.Referencia))
                throw new ReglaDominioException($"La Solicitud {solicitudId} no debe tener la Referencia en blanco");

            if (string.IsNullOrWhiteSpace(dto.Medio))
                throw new ReglaDominioException($"La Solicitud {solicitudId} debe tener un medio seleccionado");

            var solicitud = await _uow.Solicitudes.GetByIdConDetallesAsync(solicitudId, ct)
                ?? throw new KeyNotFoundException($"No se encontró la solicitud {solicitudId}");

            var tesorero = await _uow.Usuarios.GetByIdAsync(dto.ResponsableId, ct)
                ?? throw new KeyNotFoundException($"Tesorero {dto.ResponsableId} no encontrado.");

            var mov = new MovimientoTesoreria
            {
                MontoPagado = solicitud.MontoTotal,
                Medio = dto.Medio,
                Referencia = dto.Referencia
            };
            try
            {
                solicitud.RegistrarPago(mov, tesorero);
            }catch(ReglaDominioException rde)
            {
                return new ResultadoTransicionDto(
                    solicitud.Id,
                    solicitud.Estado.ToString(),
                    "Proceso No habilitiado: " + rde.Message
                );
            }
            
            await _uow.SaveChangesAsync(ct);
            return new ResultadoTransicionDto(
                solicitud.Id,
                solicitud.Estado.ToString(),
                "La Solicitud fue Pagada"
                );
        }

        public async Task<IReadOnlyList<TimelineItemDto>> ObtenerTimelineAsync(int id, CancellationToken ct = default)
        {
            var solicitud = await _uow.Solicitudes.GetByIdConDetallesAsync(id, ct)
                ?? throw new KeyNotFoundException($"No se encontro la solicitud {id}");

            return solicitud.Aprobaciones
                .OrderBy(e => e.Fecha)
                .Select(a => new TimelineItemDto(
                    a.Id,
                    a.SolicitudId,
                    a.EstadoResultante,
                    a.EstadoResultante.ToString(),
                    a.ResponsableId,
                    a.Responsable?.Nombre ?? $"Usuario #{a.ResponsableId}",
                    a.Comentario,
                    a.Fecha
                    )
                ).ToList();
        }
    }
}
