using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SolicitudesApp.Domain.Entidades;

namespace SolicitudesApp.Infrastructure.Persistencia
{
    public class RolConfiguration : IEntityTypeConfiguration<Rol>
    {
        public void Configure(EntityTypeBuilder<Rol> builder)
        {
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Nombre).IsRequired().HasMaxLength(50);

            builder.HasData(
                new Rol { Id = 1, Nombre = "Usuario" },
                new Rol { Id = 2, Nombre = "Revisor"},
                new Rol { Id = 3, Nombre = "Aprobador"},
                new Rol { Id = 4, Nombre = "Tesoreria"}

                );
        }
    }
}
