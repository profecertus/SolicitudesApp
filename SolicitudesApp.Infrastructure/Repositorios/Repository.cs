using Microsoft.EntityFrameworkCore;
using SolicitudesApp.Application.Interfaces;
using SolicitudesApp.Domain.Entidades;
using SolicitudesApp.Infrastructure.Persistencia;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace SolicitudesApp.Infrastructure.Repositorios
{
    public class Repository<T> : IRepository<T> where T : class 
    {
        protected readonly SolicitudesDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(SolicitudesDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(int id, CancellationToken ct = default) =>
            await _dbSet.FindAsync(new object?[] { id }, cancellationToken: ct);

        public async Task<IReadOnlyList<T>> ListAllAsync(CancellationToken ct = default) =>
            await _dbSet.AsNoTracking().ToListAsync(ct);

        public async Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default) =>
            await _dbSet.AsNoTracking().Where(predicate).ToListAsync(ct);

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default) =>
            await _dbSet.AnyAsync(predicate, ct);

        public async Task<T> AddAsync(T entity, CancellationToken ct = default)
        {
            await _dbSet.AddAsync(entity, ct);
            return entity;
        }

        public Task UpdateAsync(T entity, CancellationToken ct = default)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return Task.CompletedTask;
        }        

        public Task DeleteAsync(T entity, CancellationToken ct = default)
        { 
            /*
            if ( entity is EntidadAuditable modificado)
            {
                modificado.ModificadoPor = "0";
                _context.Entry(entity).State = EntityState.Modified;
            }*/
            _dbSet.Remove(entity);
            return Task.CompletedTask;
        }
    }
}
