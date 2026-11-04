using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SolicitudesApp.Domain.Entidades;

namespace SolicitudesApp.Infrastructure.Persistencia
{
    // Historial de transiciones coherente con Solicitud.PuedePasarA:
    // Borrador -> Enviada -> EnRevision -> Aprobada/Rechazada, Aprobada -> Pagada
    // Responsables: 1 Ana / 5 Elena (solicitantes), 2 Bruno (revisor), 3 Carla (aprobadora), 4 Diego (tesoreria)
    public class AprobacionConfiguration : IEntityTypeConfiguration<Aprobacion>
    {
        public void Configure(EntityTypeBuilder<Aprobacion> builder)
        {
            builder.HasData(
                // Solicitud 2: Enviada
                Semilla(1, 2, 5, EstadoSolicitud.Enviada, "Envio de solicitud", 2, 10),
                // Solicitud 3: EnRevision
                Semilla(2, 3, 1, EstadoSolicitud.Enviada, "Envio de solicitud", 3, 10),
                Semilla(3, 3, 2, EstadoSolicitud.EnRevision, "Revision iniciada", 4, 11),
                // Solicitud 4: Aprobada
                Semilla(4, 4, 5, EstadoSolicitud.Enviada, "Envio de solicitud", 4, 10),
                Semilla(5, 4, 2, EstadoSolicitud.EnRevision, "Revision iniciada", 5, 11),
                Semilla(6, 4, 3, EstadoSolicitud.Aprobada, "Aprobada por nivel Gerencia", 6, 15),
                // Solicitud 5: Rechazada
                Semilla(7, 5, 1, EstadoSolicitud.Enviada, "Envio de solicitud", 5, 10),
                Semilla(8, 5, 2, EstadoSolicitud.EnRevision, "Revision iniciada", 6, 11),
                Semilla(9, 5, 3, EstadoSolicitud.Rechazada, "Presupuesto de viajes agotado para este periodo", 7, 16),
                // Solicitud 6: Pagada
                Semilla(10, 6, 5, EstadoSolicitud.Enviada, "Envio de solicitud", 6, 10),
                Semilla(11, 6, 2, EstadoSolicitud.EnRevision, "Revision iniciada", 7, 11),
                Semilla(12, 6, 3, EstadoSolicitud.Aprobada, "Aprobada por nivel Gerencia", 8, 15),
                Semilla(13, 6, 4, EstadoSolicitud.Pagada, "Pago procesado: Transferencia - Ref: TRX-2026-0001", 9, 10)
            );
        }

        private static Aprobacion Semilla(int id, int solicitudId, int responsableId,
            EstadoSolicitud estado, string comentario, int dia, int hora) => new()
        {
            Id = id,
            SolicitudId = solicitudId,
            ResponsableId = responsableId,
            EstadoResultante = estado,
            Comentario = comentario,
            Fecha = new DateTime(2026, 10, dia, hora, 0, 0, DateTimeKind.Utc)
        };
    }
}
