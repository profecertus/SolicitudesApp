using Microsoft.EntityFrameworkCore;
using SolicitudesApp.Domain.Entidades;

namespace SolicitudesApp.Infrastructure.Persistencia
{
    public class SolicitudesDbContext:DbContext
    {
        public SolicitudesDbContext(DbContextOptions<SolicitudesDbContext> options):base(options) { }

        public DbSet<Solicitud> Solicitudes => Set<Solicitud>();
        public DbSet<SolicitudDetalle> SolicitudDetalles => Set<SolicitudDetalle>();
        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<Rol> Roles => Set<Rol>();
        public DbSet<UsuarioRol> UsuarioRoles => Set<UsuarioRol>();
        public DbSet<Aprobacion> Aprobaciones => Set<Aprobacion>();
        public DbSet<MovimientoTesoreria> MovimientoTesoreria => Set<MovimientoTesoreria>();
        public DbSet<CentroCosto> CentroCosto => Set<CentroCosto>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SolicitudesDbContext).Assembly);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) {
            var entradas = ChangeTracker.Entries<EntidadAuditable>();

            foreach(var entrada in entradas)
            {
                //Evaluamos los estados de la entidad, si es nuevo
                if(entrada.State == EntityState.Added)
                {
                    entrada.Entity.FechaCreacion = DateTime.UtcNow;
                    if (string.IsNullOrEmpty(entrada.Entity.CreadoPor))
                        entrada.Entity.CreadoPor = "sistema";
                } //Si es modificado
                else if (entrada.State == EntityState.Modified)
                {
                    entrada.Entity.FechaModificacion = DateTime.UtcNow;
                    if (string.IsNullOrEmpty(entrada.Entity.ModificadoPor))
                        entrada.Entity.ModificadoPor = "sistema";
                }
            }
            return base.SaveChangesAsync(cancellationToken);

        }
    }
}
