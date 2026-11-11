using Microsoft.EntityFrameworkCore;
using SolicitudesApp.Domain.Entidades;

namespace SolicitudesApp.Infrastructure.Persistencia
{
    public static class DbInitializer
    {
        public static async Task InicializarUsuariosSemillaAsync(SolicitudesDbContext db)
        {
            await db.Database.EnsureCreatedAsync();

            // Si la BD ya existía de una versión previa sin la columna Username, la agregamos de forma segura
            await db.Database.ExecuteSqlRawAsync(@"
                IF COL_LENGTH('Usuarios', 'Username') IS NULL
                BEGIN
                    ALTER TABLE Usuarios ADD Username NVARCHAR(50) NOT NULL DEFAULT '';
                END
            ");

            var semillas = new (int Id, string Username, string Nombre, string Correo, string PasswordHash, int RolId)[]
            {
                (1, "ana.torres", "Ana Torres", "ana.torres@solicitudes.com", UsuarioConfiguration.HashPassword123456, 1),
                (2, "bruno.diaz", "Bruno Diaz", "bruno.diaz@solicitudes.com", UsuarioConfiguration.HashPassword123456, 2),
                (3, "carla.ruiz", "Carla Ruiz", "carla.ruiz@solicitudes.com", UsuarioConfiguration.HashPassword123456, 3),
                (4, "diego.paz", "Diego Paz", "diego.paz@solicitudes.com", UsuarioConfiguration.HashPassword123456, 4),
                (5, "elena.soto", "Elena Soto", "elena.soto@solicitudes.com", UsuarioConfiguration.HashPassword123456, 1),
                (6, "admin", "Administrador Sistema", "admin@solicitudes.com", UsuarioConfiguration.HashPasswordAdmin123, 3)
            };

            var fechaSemilla = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var huboCambios = false;

            foreach (var item in semillas)
            {
                var existente = await db.Usuarios
                    .FirstOrDefaultAsync(u => u.Id == item.Id || u.Correo == item.Correo);

                if (existente is null)
                {
                    var nuevo = new Usuario
                    {
                        Username = item.Username,
                        Nombre = item.Nombre,
                        Correo = item.Correo,
                        PasswordHash = item.PasswordHash,
                        Activo = true,
                        FechaCreacion = fechaSemilla,
                        CreadoPor = "seed"
                    };
                    db.Usuarios.Add(nuevo);
                    await db.SaveChangesAsync();

                    if (!await db.UsuarioRoles.AnyAsync(ur => ur.UsuarioId == nuevo.Id && ur.RolId == item.RolId))
                    {
                        db.UsuarioRoles.Add(new UsuarioRol
                        {
                            UsuarioId = nuevo.Id,
                            RolId = item.RolId,
                            FechaAsignacion = fechaSemilla
                        });
                        await db.SaveChangesAsync();
                    }
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(existente.Username))
                    {
                        existente.Username = item.Username;
                        huboCambios = true;
                    }

                    if (string.IsNullOrWhiteSpace(existente.PasswordHash) ||
                        existente.PasswordHash == "seed-sin-autenticacion")
                    {
                        existente.PasswordHash = item.PasswordHash;
                        huboCambios = true;
                    }
                }
            }

            if (huboCambios)
            {
                await db.SaveChangesAsync();
            }
        }
    }
}
