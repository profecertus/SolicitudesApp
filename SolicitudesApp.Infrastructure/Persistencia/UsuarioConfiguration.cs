using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SolicitudesApp.Domain.Entidades;

namespace SolicitudesApp.Infrastructure.Persistencia
{
    public class UsuarioConfiguration:IEntityTypeConfiguration<Usuario>
    {
        private static readonly DateTime FechaSemilla = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuarios");
            builder.HasKey(u => u.Id);
            builder.HasMany(u => u.Roles).WithMany(r => r.Usuarios).
                UsingEntity<UsuarioRol>(
                    relacion =>
                        relacion.HasOne(ur => ur.Rol).WithMany().HasForeignKey(ur => ur.RolId).OnDelete(DeleteBehavior.Restrict),
                    relacion =>
                        relacion.HasOne(ur => ur.Usuario).WithMany().HasForeignKey(ur => ur.UsuarioId).OnDelete(DeleteBehavior.Restrict),
                    relacion => {
                        relacion.ToTable("UsuarioRoles");
                        relacion.HasKey(ur => new { ur.UsuarioId, ur.RolId });
                        relacion.Property(ur => ur.FechaAsignacion).IsRequired();
                        relacion.HasData(
                            new UsuarioRol { UsuarioId = 1, RolId = 1, FechaAsignacion = FechaSemilla },
                            new UsuarioRol { UsuarioId = 2, RolId = 2, FechaAsignacion = FechaSemilla },
                            new UsuarioRol { UsuarioId = 3, RolId = 3, FechaAsignacion = FechaSemilla },
                            new UsuarioRol { UsuarioId = 4, RolId = 4, FechaAsignacion = FechaSemilla },
                            new UsuarioRol { UsuarioId = 5, RolId = 1, FechaAsignacion = FechaSemilla }
                        );
                        }
                );

            // PasswordHash es un marcador: la API aun no tiene autenticacion
            builder.HasData(
                new Usuario { Id = 1, Nombre = "Ana Torres", Correo = "ana.torres@solicitudes.com", PasswordHash = "seed-sin-autenticacion", Activo = true, FechaCreacion = FechaSemilla, CreadoPor = "seed" },
                new Usuario { Id = 2, Nombre = "Bruno Diaz", Correo = "bruno.diaz@solicitudes.com", PasswordHash = "seed-sin-autenticacion", Activo = true, FechaCreacion = FechaSemilla, CreadoPor = "seed" },
                new Usuario { Id = 3, Nombre = "Carla Ruiz", Correo = "carla.ruiz@solicitudes.com", PasswordHash = "seed-sin-autenticacion", Activo = true, FechaCreacion = FechaSemilla, CreadoPor = "seed" },
                new Usuario { Id = 4, Nombre = "Diego Paz", Correo = "diego.paz@solicitudes.com", PasswordHash = "seed-sin-autenticacion", Activo = true, FechaCreacion = FechaSemilla, CreadoPor = "seed" },
                new Usuario { Id = 5, Nombre = "Elena Soto", Correo = "elena.soto@solicitudes.com", PasswordHash = "seed-sin-autenticacion", Activo = true, FechaCreacion = FechaSemilla, CreadoPor = "seed" }
            );
        }
    }
}
