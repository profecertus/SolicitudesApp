using Microsoft.EntityFrameworkCore;
using SolicitudesApp.Application.DTOs;
using SolicitudesApp.Application.Interfaces;
using SolicitudesApp.Domain.Entidades;
using SolicitudesApp.Infrastructure.Persistencia;


namespace SolicitudesApp.Infrastructure.Repositorios
{
    public class SolicitudRepository: Repository<Solicitud>, ISolicitudRepository
    {
        public SolicitudRepository(SolicitudesDbContext context) : base(context) { }

        public async Task<Solicitud?> GetByIdConDetallesAsync(int id, CancellationToken ct = default)
        {
            return await _dbSet
                .Include(s => s.Detalles)
                .Include(s => s.Solicitante)
                .Include(s => s.CentroCosto)
                .Include(s => s.Aprobaciones)
                .FirstOrDefaultAsync(s => s.Id == id, ct);
        }

        public async Task<PagedResult<Solicitud>> ObtenerPaginadoAsync(int pagina, int tamanoPagina, EstadoSolicitud? estado, CancellationToken ct = default)
        {
            var query = _dbSet
                .Include(s => s.Detalles)
                .Include(s => s.Solicitante)
                .Include(s => s.CentroCosto)
                .AsNoTracking();

            if (estado.HasValue)
                query = query.Where(s => s.Estado == estado.Value);

            var total = await query.CountAsync(ct);
            var items = await query
                .OrderByDescending(s => s.FechaCreacion)
                .Skip((pagina - 1) * tamanoPagina)
                .Take(tamanoPagina)
                .ToListAsync(ct);
            var totalPaginas = (int)Math.Ceiling(total / (double)tamanoPagina);
            return new PagedResult<Solicitud>(items, total, pagina, tamanoPagina, totalPaginas);
        }
    }
}
