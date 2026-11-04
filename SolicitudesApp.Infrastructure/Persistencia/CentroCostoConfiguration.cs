using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SolicitudesApp.Domain.Entidades;

namespace SolicitudesApp.Infrastructure.Persistencia
{
    public class CentroCostoConfiguration : IEntityTypeConfiguration<CentroCosto>
    {
        public void Configure(EntityTypeBuilder<CentroCosto> builder)
        {
            builder.ToTable("CentroCosto");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Codigo).IsRequired().HasMaxLength(20);
            builder.Property(c => c.Nombre).IsRequired().HasMaxLength(100);
            builder.Property(c => c.CreadoPor).IsRequired().HasMaxLength(20);
            //builder.Property(c => c.FechaCreacion).IsRequired().HasDefaultValueSql("GETUTCDATE()");
            
            builder.HasData(
                new
                {
                    Id = 1,
                    Codigo = "CC-TI-01",
                    Nombre = "Tecnologías de la Información",
                    Activo = true,
                    FechaCreacion = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    CreadoPor = "seed"
                },
                new
                {
                    Id = 2,
                    Codigo = "CC-OPS-02",
                    Nombre = "Operaciones y Logística",
                    Activo = true,
                    FechaCreacion = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    CreadoPor = "seed"
                },
                new
                {
                    Id = 3,
                    Codigo = "CC-VEN-03",
                    Nombre = "Ventas y Marketing",
                    Activo = true,
                    FechaCreacion = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    CreadoPor = "seed"
                }
                );
        }
    }
    
}
