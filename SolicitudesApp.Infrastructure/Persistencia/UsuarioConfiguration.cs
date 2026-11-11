using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SolicitudesApp.Domain.Entidades;

namespace SolicitudesApp.Infrastructure.Persistencia
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        private static readonly DateTime FechaSemilla = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        // Hash SHA-256 determinista para "123456" (Salt: SolicitudesApp_Salt_2026!)
        public const string HashPassword123456 = "0f80f19b3ab7eb5c11371e4727ec69e325443dc2d0d0918e44739e36a3b08377";
        // Hash SHA-256 determinista para "admin123" (Salt: SolicitudesApp_Salt_2026!)
        public const string HashPasswordAdmin123 = "1802a3950a346fe794f67d6584832b7066519637e7b40508c0cd6daf00277b33";

        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuarios");
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Username).IsRequired().HasMaxLength(50);
            builder.Property(u => u.Nombre).IsRequired().HasMaxLength(120);
            builder.Property(u => u.Correo).IsRequired().HasMaxLength(150);
            builder.Property(u => u.PasswordHash).IsRequired();

            builder.HasMany(u => u.Roles).WithMany(r => r.Usuarios).
                UsingEntity<UsuarioRol>(
                    relacion =>
                        relacion.HasOne(ur => ur.Rol).WithMany().HasForeignKey(ur => ur.RolId).OnDelete(DeleteBehavior.Restrict),
                    relacion =>
                        relacion.HasOne(ur => ur.Usuario).WithMany().HasForeignKey(ur => ur.UsuarioId).OnDelete(DeleteBehavior.Restrict),
                    relacion =>
                    {
                        relacion.ToTable("UsuarioRoles");
                        relacion.HasKey(ur => new { ur.UsuarioId, ur.RolId });
                        relacion.Property(ur => ur.FechaAsignacion).IsRequired();
                        relacion.HasData(
                            new UsuarioRol { UsuarioId = 1, RolId = 1, FechaAsignacion = FechaSemilla },
                            new UsuarioRol { UsuarioId = 2, RolId = 2, FechaAsignacion = FechaSemilla },
                            new UsuarioRol { UsuarioId = 3, RolId = 3, FechaAsignacion = FechaSemilla },
                            new UsuarioRol { UsuarioId = 4, RolId = 4, FechaAsignacion = FechaSemilla },
                            new UsuarioRol { UsuarioId = 5, RolId = 1, FechaAsignacion = FechaSemilla },
                            new UsuarioRol { UsuarioId = 6, RolId = 3, FechaAsignacion = FechaSemilla }
                        );
                    }
                );

            // Registros semilla con usuario (Username) y contraseña hasheada en BD
            // Usuarios 1..5 -> password: "123456" | Usuario 6 (admin) -> password: "admin123"
            builder.HasData(
                new Usuario { Id = 1, Username = "ana.torres", Nombre = "Ana Torres", Correo = "ana.torres@solicitudes.com", PasswordHash = HashPassword123456, Activo = true, FechaCreacion = FechaSemilla, CreadoPor = "seed" },
                new Usuario { Id = 2, Username = "bruno.diaz", Nombre = "Bruno Diaz", Correo = "bruno.diaz@solicitudes.com", PasswordHash = HashPassword123456, Activo = true, FechaCreacion = FechaSemilla, CreadoPor = "seed" },
                new Usuario { Id = 3, Username = "carla.ruiz", Nombre = "Carla Ruiz", Correo = "carla.ruiz@solicitudes.com", PasswordHash = HashPassword123456, Activo = true, FechaCreacion = FechaSemilla, CreadoPor = "seed" },
                new Usuario { Id = 4, Username = "diego.paz", Nombre = "Diego Paz", Correo = "diego.paz@solicitudes.com", PasswordHash = HashPassword123456, Activo = true, FechaCreacion = FechaSemilla, CreadoPor = "seed" },
                new Usuario { Id = 5, Username = "elena.soto", Nombre = "Elena Soto", Correo = "elena.soto@solicitudes.com", PasswordHash = HashPassword123456, Activo = true, FechaCreacion = FechaSemilla, CreadoPor = "seed" },
                new Usuario { Id = 6, Username = "admin", Nombre = "Administrador Sistema", Correo = "admin@solicitudes.com", PasswordHash = HashPasswordAdmin123, Activo = true, FechaCreacion = FechaSemilla, CreadoPor = "seed" }
            );
        }
    }
}
