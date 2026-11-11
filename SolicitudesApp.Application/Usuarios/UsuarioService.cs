using FluentValidation;
using SolicitudesApp.Application.Auth;
using SolicitudesApp.Application.DTOs;
using SolicitudesApp.Application.Interfaces;
using SolicitudesApp.Application.Roles;
using SolicitudesApp.Application.Services;
using SolicitudesApp.Domain.Entidades;
using SolicitudesApp.Domain.Excepciones;

namespace SolicitudesApp.Application.Usuarios
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUnitOfWork _uow;
        private readonly IValidator<CrearUsuarioDto> _crearValidator;
        private readonly IValidator<ActualizarUsuarioDto> _actualizarValidator;

        public UsuarioService(IUnitOfWork uow,
            IValidator<CrearUsuarioDto> crearValidator,
            IValidator<ActualizarUsuarioDto> actualizarValidator)
        {
            _uow = uow;
            _crearValidator = crearValidator;
            _actualizarValidator = actualizarValidator;
        }

        public async Task<IReadOnlyList<UsuarioDto>> ListarAsync(CancellationToken ct = default) =>
            (await _uow.Usuarios.ListarConRolesAsync(ct)).Select(MapearADto).ToList();

        public async Task<UsuarioDto?> ObtenerPorIdAsync(int id, CancellationToken ct = default)
        {
            var usuario = await _uow.Usuarios.GetByIdConRolesAsync(id, ct);
            return usuario is null ? null : MapearADto(usuario);
        }

        public async Task<UsuarioDto> CrearAsync(CrearUsuarioDto dto, CancellationToken ct = default)
        {
            await _crearValidator.ValidateAndThrowAsync(dto, ct);
            var correoNormalizado = dto.Correo.Trim().ToLowerInvariant();
            var usernameNormalizado = string.IsNullOrWhiteSpace(dto.Username)
                ? correoNormalizado.Split('@')[0]
                : dto.Username.Trim().ToLowerInvariant();

            await ValidarCorreoUnicoAsync(correoNormalizado, null, ct);
            await ValidarUsernameUnicoAsync(usernameNormalizado, null, ct);

            var passwordPlano = string.IsNullOrWhiteSpace(dto.Password) ? "123456" : dto.Password.Trim();

            var usuario = new Usuario
            {
                Username = usernameNormalizado,
                Nombre = dto.Nombre.Trim(),
                Correo = correoNormalizado,
                PasswordHash = PasswordHasher.Hash(passwordPlano),
                Activo = true
            };
            foreach (var rolId in (dto.RolIds ?? []).Distinct())
            {
                var rol = await _uow.Roles.GetByIdAsync(rolId, ct)
                    ?? throw new KeyNotFoundException($"No se encontró el rol con Id {rolId}");
                usuario.Roles.Add(rol);
            }

            await _uow.Usuarios.AddAsync(usuario, ct);
            await _uow.SaveChangesAsync(ct);
            return MapearADto(usuario);
        }

        public async Task ActualizarAsync(int id, ActualizarUsuarioDto dto, CancellationToken ct = default)
        {
            await _actualizarValidator.ValidateAndThrowAsync(dto, ct);
            var usuario = await _uow.Usuarios.GetByIdAsync(id, ct)
                ?? throw new KeyNotFoundException($"No se encontró el usuario con Id {id}");
            var correoNormalizado = dto.Correo.Trim().ToLowerInvariant();
            await ValidarCorreoUnicoAsync(correoNormalizado, id, ct);

            if (!string.IsNullOrWhiteSpace(dto.Username))
            {
                var usernameNormalizado = dto.Username.Trim().ToLowerInvariant();
                await ValidarUsernameUnicoAsync(usernameNormalizado, id, ct);
                usuario.Username = usernameNormalizado;
            }

            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                usuario.PasswordHash = PasswordHasher.Hash(dto.Password.Trim());
            }

            usuario.Nombre = dto.Nombre.Trim();
            usuario.Correo = correoNormalizado;
            usuario.Activo = dto.Activo;
            await _uow.Usuarios.UpdateAsync(usuario, ct);
            await _uow.SaveChangesAsync(ct);
        }

        public async Task EliminarAsync(int id, CancellationToken ct = default)
        {
            var usuario = await _uow.Usuarios.GetByIdAsync(id, ct)
                ?? throw new KeyNotFoundException($"No se encontró el usuario con Id {id}");
            if (await _uow.Solicitudes.AnyAsync(s => s.SolicitanteId == id, ct)
                || await _uow.Aprobaciones.AnyAsync(a => a.ResponsableId == id, ct))
                throw new ReglaDominioException("No se puede eliminar un usuario con solicitudes o aprobaciones registradas. Desactívelo en su lugar.");

            foreach (var asignacion in await _uow.UsuarioRoles.FindAsync(ur => ur.UsuarioId == id, ct))
                await _uow.UsuarioRoles.DeleteAsync(asignacion, ct);
            await _uow.Usuarios.DeleteAsync(usuario, ct);
            await _uow.SaveChangesAsync(ct);
        }

        public async Task<IReadOnlyList<RolDto>> ListarRolesAsync(int usuarioId, CancellationToken ct = default)
        {
            var usuario = await _uow.Usuarios.GetByIdConRolesAsync(usuarioId, ct)
                ?? throw new KeyNotFoundException($"No se encontró el usuario con Id {usuarioId}");
            return usuario.Roles.OrderBy(r => r.Id).Select(RolService.MapearADto).ToList();
        }

        public async Task AsignarRolAsync(int usuarioId, int rolId, CancellationToken ct = default)
        {
            _ = await _uow.Usuarios.GetByIdAsync(usuarioId, ct)
                ?? throw new KeyNotFoundException($"No se encontró el usuario con Id {usuarioId}");
            _ = await _uow.Roles.GetByIdAsync(rolId, ct)
                ?? throw new KeyNotFoundException($"No se encontró el rol con Id {rolId}");
            if (await _uow.UsuarioRoles.AnyAsync(ur => ur.UsuarioId == usuarioId && ur.RolId == rolId, ct))
                throw new ReglaDominioException($"El usuario {usuarioId} ya tiene asignado el rol {rolId}");

            await _uow.UsuarioRoles.AddAsync(new UsuarioRol { UsuarioId = usuarioId, RolId = rolId }, ct);
            await _uow.SaveChangesAsync(ct);
        }

        public async Task QuitarRolAsync(int usuarioId, int rolId, CancellationToken ct = default)
        {
            var asignacion = (await _uow.UsuarioRoles.FindAsync(ur => ur.UsuarioId == usuarioId && ur.RolId == rolId, ct))
                .FirstOrDefault()
                ?? throw new KeyNotFoundException($"El usuario {usuarioId} no tiene asignado el rol {rolId}");

            await _uow.UsuarioRoles.DeleteAsync(asignacion, ct);
            await _uow.SaveChangesAsync(ct);
        }

        private async Task ValidarCorreoUnicoAsync(string correo, int? idActual, CancellationToken ct)
        {
            var normalizado = correo.Trim().ToLowerInvariant();
            if (await _uow.Usuarios.AnyAsync(u => u.Correo == normalizado && u.Id != (idActual ?? 0), ct))
                throw new ReglaDominioException($"Ya existe un usuario con el correo {normalizado}");
        }

        private async Task ValidarUsernameUnicoAsync(string username, int? idActual, CancellationToken ct)
        {
            var normalizado = username.Trim().ToLowerInvariant();
            if (await _uow.Usuarios.AnyAsync(u => u.Username == normalizado && u.Id != (idActual ?? 0), ct))
                throw new ReglaDominioException($"Ya existe un usuario con el username {normalizado}");
        }

        private static UsuarioDto MapearADto(Usuario u) => new(
            u.Id, u.Username, u.Nombre, u.Correo, u.Activo, u.FechaCreacion,
            u.Roles.OrderBy(r => r.Id).Select(RolService.MapearADto).ToList());
    }
}
