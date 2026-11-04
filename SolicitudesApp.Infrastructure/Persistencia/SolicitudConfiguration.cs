using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SolicitudesApp.Domain.Entidades;

namespace SolicitudesApp.Infrastructure.Persistencia
{
    public  class SolicitudConfiguration:IEntityTypeConfiguration<Solicitud>
    {
        public void Configure(EntityTypeBuilder<Solicitud> builder)
        {
            builder.ToTable("Solicitudes");
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Motivo).IsRequired().HasMaxLength(250);
            builder.Property(s => s.MontoTotal).HasColumnType("decimal(18,2)");
            builder.Property(s => s.Estado).HasConversion<int>();
            builder.HasMany(s => s.Detalles).WithOne(d => d.Solicitud)
            .HasForeignKey(d => d.SolicitudId).OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(s => s.Solicitante).WithMany(u => u.Solicitudes)
            .HasForeignKey(s => s.SolicitanteId).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.CentroCosto).WithMany(c => c.Solicitudes)
            .HasForeignKey(s => s.CentroCostoId).OnDelete(DeleteBehavior.Restrict);

            // Una solicitud por cada estado. MontoTotal = suma de sus detalles activos.
            // Solicitantes: 1 Ana Torres, 5 Elena Soto
            builder.HasData(
                Semilla(1, "Compra de laptops para el equipo de desarrollo", 7000m, 1, EstadoSolicitud.Borrador, 1, new DateOnly(2026, 12, 15), 1),
                Semilla(2, "Material publicitario para campaña de fin de año", 1500m, 3, EstadoSolicitud.Enviada, 5, new DateOnly(2026, 11, 30), 2),
                Semilla(3, "Mantenimiento preventivo de montacargas", 4800m, 2, EstadoSolicitud.EnRevision, 1, new DateOnly(2026, 11, 20), 3),
                Semilla(4, "Renovación de licencias de software", 1200m, 1, EstadoSolicitud.Aprobada, 5, new DateOnly(2026, 11, 25), 4),
                Semilla(5, "Viaje a feria comercial internacional", 9000m, 3, EstadoSolicitud.Rechazada, 1, new DateOnly(2026, 12, 5), 5),
                Semilla(6, "Compra de pallets para almacén", 2250m, 2, EstadoSolicitud.Pagada, 5, new DateOnly(2026, 11, 10), 6)
            );
        }

        private static Solicitud Semilla(int id, string motivo, decimal monto, int centroCostoId,
            EstadoSolicitud estado, int solicitanteId, DateOnly fechaRequerida, int dia) => new()
        {
            Id = id,
            Motivo = motivo,
            MontoTotal = monto,
            CentroCostoId = centroCostoId,
            Estado = estado,
            SolicitanteId = solicitanteId,
            FechaRequerida = fechaRequerida,
            FechaCreacion = new DateTime(2026, 10, dia, 9, 0, 0, DateTimeKind.Utc),
            CreadoPor = "seed"
        };
    }
}
