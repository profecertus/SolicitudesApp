using System.Security.Claims;
using SolicitudesApp.Application.DTOs;
using SolicitudesApp.Domain.Entidades;

namespace SolicitudesApp.Application.Services
{
    public interface IJwtService
    {
        /// <summary>
        /// Genera un token JWT firmado con un TTL (Time-To-Live) configurable.
        /// </summary>
        JwtTokenDto GenerarToken(Usuario usuario, TimeSpan? ttl = null);

        /// <summary>
        /// Genera un token JWT especificando el TTL en minutos o segundos.
        /// </summary>
        JwtTokenDto GenerarToken(Usuario usuario, int? ttlMinutos, int? ttlSegundos = null);

        /// <summary>
        /// Valida un token JWT y retorna sus Claims si la firma y el TTL son vigentes.
        /// </summary>
        ClaimsPrincipal? ValidarToken(string token);
    }

    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginRequestDto dto, CancellationToken ct = default);
        Task<JwtTokenDto> GenerarJwtAsync(LoginRequestDto dto, CancellationToken ct = default);
    }
}
