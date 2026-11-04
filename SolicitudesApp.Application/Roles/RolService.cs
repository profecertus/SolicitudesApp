using FluentValidation;
using SolicitudesApp.Application.DTOs;
using SolicitudesApp.Application.Interfaces;
using SolicitudesApp.Application.Services;
using SolicitudesApp.Domain.Entidades;
using SolicitudesApp.Domain.Excepciones;

namespace SolicitudesApp.Application.Roles
{
    public class RolService : IRolService
    {
        private readonly IUnitOfWork _uow;
        private readonly IValidator<GuardarRolDto> _validator;

        public RolService(IUnitOfWork uow, IValidator<GuardarRolDto> validator)
        {
            _uow = uow;
            _validator = validator;
        }

        public async Task<IReadOnlyList<RolDto>> ListarAsync(CancellationToken ct = default) =>
            (await _uow.Roles.ListAllAsync(ct)).OrderBy(r => r.Id).Select(MapearADto).ToList();

        public async Task<RolDto?> ObtenerPorIdAsync(int id, CancellationToken ct = default)
        {
            var rol = await _uow.Roles.GetByIdAsync(id, ct);
            return rol is null ? null : MapearADto(rol);
        }

        public async Task<RolDto> CrearAsync(GuardarRolDto dto, CancellationToken ct = default)
        {
            await _validator.ValidateAndThrowAsync(dto, ct);
            await ValidarNombreUnicoAsync(dto.Nombre, null, ct);

            var rol = new Rol { Nombre = dto.Nombre.Trim(), LimiteAprobacion = dto.LimiteAprobacion };
            await _uow.Roles.AddAsync(rol, ct);
            await _uow.SaveChangesAsync(ct);
            return MapearADto(rol);
        }

        public async Task ActualizarAsync(int id, GuardarRolDto dto, CancellationToken ct = default)
        {
            await _validator.ValidateAndThrowAsync(dto, ct);
            var rol = await _uow.Roles.GetByIdAsync(id, ct)
                ?? throw new KeyNotFoundException($"No se encontró el rol con Id {id}");
            await ValidarNombreUnicoAsync(dto.Nombre, id, ct);

            rol.Nombre = dto.Nombre.Trim();
            rol.LimiteAprobacion = dto.LimiteAprobacion;
            await _uow.Roles.UpdateAsync(rol, ct);
            await _uow.SaveChangesAsync(ct);
        }

        public async Task EliminarAsync(int id, CancellationToken ct = default)
        {
            var rol = await _uow.Roles.GetByIdAsync(id, ct)
                ?? throw new KeyNotFoundException($"No se encontró el rol con Id {id}");
            if (await _uow.UsuarioRoles.AnyAsync(ur => ur.RolId == id, ct))
                throw new ReglaDominioException("No se puede eliminar un rol asignado a usuarios. Quite primero las asignaciones.");

            await _uow.Roles.DeleteAsync(rol, ct);
            await _uow.SaveChangesAsync(ct);
        }

        private async Task ValidarNombreUnicoAsync(string nombre, int? idActual, CancellationToken ct)
        {
            var normalizado = nombre.Trim();
            if (await _uow.Roles.AnyAsync(r => r.Nombre == normalizado && r.Id != (idActual ?? 0), ct))
                throw new ReglaDominioException($"Ya existe un rol con el nombre {normalizado}");
        }

        internal static RolDto MapearADto(Rol r) => new(r.Id, r.Nombre, r.LimiteAprobacion);
    }
}
