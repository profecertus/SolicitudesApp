using Microsoft.EntityFrameworkCore;
using SolicitudesApp.Application.Interfaces;
using SolicitudesApp.Domain.Entidades;
using SolicitudesApp.Infrastructure.Persistencia;

namespace SolicitudesApp.Infrastructure.Repositorios
{
    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(SolicitudesDbContext context) : base(context) { }

        public async Task<Usuario?> GetByIdConRolesAsync(int id, CancellationToken ct = default) =>
            await _dbSet.Include(u => u.Roles).AsNoTracking().FirstOrDefaultAsync(u => u.Id == id, ct);

        public async Task<IReadOnlyList<Usuario>> ListarConRolesAsync(CancellationToken ct = default) =>
            await _dbSet.Include(u => u.Roles).AsNoTracking().OrderBy(u => u.Id).ToListAsync(ct);
    }
}
