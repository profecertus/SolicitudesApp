using Microsoft.EntityFrameworkCore;
using SolicitudesApp.Application.Interfaces;
using SolicitudesApp.Domain.Entidades;
using SolicitudesApp.Infrastructure.Persistencia;

namespace SolicitudesApp.Infrastructure.Repositorios
{
    public class AprobacionRepository : Repository<Aprobacion>, IAprobacionRepository
    {
        public AprobacionRepository(SolicitudesDbContext context) : base(context) { }

        public async Task<Aprobacion?> GetByIdConResponsableAsync(int id, CancellationToken ct = default) =>
            await _dbSet.Include(a => a.Responsable).AsNoTracking().FirstOrDefaultAsync(a => a.Id == id, ct);

        public async Task<IReadOnlyList<Aprobacion>> ListarConResponsableAsync(int? solicitudId, CancellationToken ct = default)
        {
            var query = _dbSet.Include(a => a.Responsable).AsNoTracking();
            if (solicitudId.HasValue)
                query = query.Where(a => a.SolicitudId == solicitudId.Value);
            return await query.OrderBy(a => a.SolicitudId).ThenBy(a => a.Fecha).ToListAsync(ct);
        }
    }
}
