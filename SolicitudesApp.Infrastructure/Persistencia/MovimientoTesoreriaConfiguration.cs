using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SolicitudesApp.Domain.Entidades;

namespace SolicitudesApp.Infrastructure.Persistencia
{
    public class MovimientoTesoreriaConfiguration : IEntityTypeConfiguration<MovimientoTesoreria>
    {
        public void Configure(EntityTypeBuilder<MovimientoTesoreria> builder)
        {
            // Pago de la solicitud 6 (unica en estado Pagada), registrado por Diego Paz (tesoreria)
            builder.HasData(new MovimientoTesoreria
            {
                Id = 1,
                SolicitudId = 6,
                MontoPagado = 2250m,
                Medio = "Transferencia",
                Referencia = "TRX-2026-0001",
                FechaPago = new DateTime(2026, 10, 9, 10, 0, 0, DateTimeKind.Utc),
                FechaCreacion = new DateTime(2026, 10, 9, 10, 0, 0, DateTimeKind.Utc),
                CreadoPor = "diego.paz@solicitudes.com"
            });
        }
    }
}
