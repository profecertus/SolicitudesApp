using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SolicitudesApp.Domain.Entidades;

namespace SolicitudesApp.Infrastructure.Persistencia
{
    public class SolicitudDetalleConfiguration : IEntityTypeConfiguration<SolicitudDetalle>
    {
        public void Configure(EntityTypeBuilder<SolicitudDetalle> builder)
        {
            builder.HasData(
                Semilla(1, 1, "Laptop 16GB RAM / 512GB SSD", 2, 3500m, 1),
                Semilla(2, 2, "Volantes impresos (paquete x100)", 2, 500m, 2),
                Semilla(3, 2, "Banner roll-up", 2, 250m, 2),
                Semilla(4, 3, "Servicio de mantenimiento preventivo", 1, 4800m, 3),
                Semilla(5, 4, "Licencia anual de software de oficina", 10, 120m, 4),
                Semilla(6, 5, "Pasajes y viáticos para feria", 1, 9000m, 5),
                Semilla(7, 6, "Pallet de madera estándar", 50, 45m, 6)
            );
        }

        private static SolicitudDetalle Semilla(int id, int solicitudId, string concepto,
            int cantidad, decimal precioUnitario, int dia) => new()
        {
            Id = id,
            SolicitudId = solicitudId,
            Concepto = concepto,
            Cantidad = cantidad,
            PrecioUnitario = precioUnitario,
            Activo = 'A',
            FechaCreacion = new DateTime(2026, 10, dia, 9, 0, 0, DateTimeKind.Utc),
            CreadoPor = "seed"
        };
    }
}
