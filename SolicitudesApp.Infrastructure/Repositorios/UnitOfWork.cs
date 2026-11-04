using SolicitudesApp.Application.Interfaces;
using SolicitudesApp.Domain.Entidades;
using SolicitudesApp.Infrastructure.Persistencia;
using System;
using System.Collections.Generic;
using System.Text;

namespace SolicitudesApp.Infrastructure.Repositorios
{
    public  class UnitOfWork : IUnitOfWork
    {
        private readonly SolicitudesDbContext _context;
        private ISolicitudRepository? _solicitudes;
        private IRepository<CentroCosto>? _centrosCosto;
        private IUsuarioRepository? _usuarios;
        private IAprobacionRepository? _aprobaciones;
        private IRepository<Rol>? _roles;
        private IRepository<UsuarioRol>? _usuarioRoles;
        private IRepository<SolicitudDetalle>? _solicitudDetalles;
        private IRepository<MovimientoTesoreria>? _movimientosTesoreria;

        public UnitOfWork(SolicitudesDbContext context) => _context = context;

        public ISolicitudRepository Solicitudes => _solicitudes ??= new SolicitudRepository(_context);

        public IRepository<CentroCosto> CentrosCosto => _centrosCosto ??= new Repository<CentroCosto>(_context);  
        public IUsuarioRepository Usuarios => _usuarios ??= new UsuarioRepository(_context);
        public IAprobacionRepository Aprobaciones => _aprobaciones ??= new AprobacionRepository(_context);
        public IRepository<Rol> Roles => _roles ??= new Repository<Rol>(_context);
        public IRepository<UsuarioRol> UsuarioRoles => _usuarioRoles ??= new Repository<UsuarioRol>(_context);
        public IRepository<SolicitudDetalle> SolicitudDetalles => _solicitudDetalles ??= new Repository<SolicitudDetalle>(_context);
        public IRepository<MovimientoTesoreria> MovimientosTesoreria => _movimientosTesoreria ??= new Repository<MovimientoTesoreria>(_context);


        public async Task<int> SaveChangesAsync(CancellationToken ct = default) => await _context.SaveChangesAsync(ct);

        public void Dispose() => _context.Dispose();

    }
}
