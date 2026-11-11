using SolicitudesApp.Application.DTOs;
using SolicitudesApp.Application.Interfaces;
using SolicitudesApp.Application.Services;
using SolicitudesApp.Domain.Entidades;

namespace SolicitudesApp.Application.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _uow;
        private readonly IJwtService _jwtService;

        public AuthService(IUnitOfWork uow, IJwtService jwtService)
        {
            _uow = uow;
            _jwtService = jwtService;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto dto, CancellationToken ct = default)
        {
            var usuario = await AutenticarUsuarioAsync(dto, ct);

            // Para pruebas: si inicia sesión "ana.torres" y no especificó otro TTL, su JWT tiene un TTL de 20 segundos
            var ttlSegundos = dto.TtlSegundos;
            var ttlMinutos = dto.TtlMinutos;
            if (ttlSegundos is null && ttlMinutos is null &&
                string.Equals(usuario.Username, "ana.torres", StringComparison.OrdinalIgnoreCase))
            {
                ttlSegundos = 20;
            }

            var jwtResult = _jwtService.GenerarToken(usuario, ttlMinutos, ttlSegundos);

            var roles = usuario.Roles
                .OrderBy(r => r.Id)
                .Select(r => r.Nombre)
                .ToList();

            var rolPrincipal = roles.FirstOrDefault() ?? "Usuario";

            return new AuthResponseDto(
                Id: usuario.Id,
                Username: usuario.Username,
                Nombre: usuario.Nombre,
                Correo: usuario.Correo,
                Email: usuario.Correo,
                Rol: rolPrincipal,
                Roles: roles,
                Token: jwtResult.Token,
                TokenType: jwtResult.TokenType,
                TtlSeconds: jwtResult.TtlSeconds,
                TtlMinutes: jwtResult.TtlMinutes,
                FechaEmisionUtc: jwtResult.FechaEmisionUtc,
                Expiracion: jwtResult.FechaExpiracionUtc
            );
        }

        public async Task<JwtTokenDto> GenerarJwtAsync(LoginRequestDto dto, CancellationToken ct = default)
        {
            var usuario = await AutenticarUsuarioAsync(dto, ct);
            var ttlSegundos = dto.TtlSegundos;
            var ttlMinutos = dto.TtlMinutos;
            if (ttlSegundos is null && ttlMinutos is null &&
                string.Equals(usuario.Username, "ana.torres", StringComparison.OrdinalIgnoreCase))
            {
                ttlSegundos = 20;
            }
            return _jwtService.GenerarToken(usuario, ttlMinutos, ttlSegundos);
        }

        private async Task<Usuario> AutenticarUsuarioAsync(LoginRequestDto dto, CancellationToken ct)
        {
            var identificador = dto.ObtenerIdentificador();
            if (string.IsNullOrWhiteSpace(identificador) || string.IsNullOrWhiteSpace(dto.Password))
            {
                throw new UnauthorizedAccessException("Debe proporcionar usuario/correo y contraseña.");
            }

            var usuario = await _uow.Usuarios.ObtenerPorUsuarioOCorreoAsync(identificador, ct);
            if (usuario is null)
            {
                throw new UnauthorizedAccessException("Credenciales inválidas: usuario o contraseña incorrectos.");
            }

            if (!usuario.Activo)
            {
                throw new UnauthorizedAccessException($"El usuario '{usuario.Username}' se encuentra inactivo.");
            }

            if (!PasswordHasher.Verify(dto.Password, usuario.PasswordHash))
            {
                throw new UnauthorizedAccessException("Credenciales inválidas: usuario o contraseña incorrectos.");
            }

            return usuario;
        }
    }
}
