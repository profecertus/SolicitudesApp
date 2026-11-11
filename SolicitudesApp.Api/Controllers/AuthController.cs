using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolicitudesApp.Application.DTOs;
using SolicitudesApp.Application.Services;

namespace SolicitudesApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Autentica un usuario contra la Base de Datos (por Username o Correo y Password) y genera un JWT con TTL.
        /// </summary>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginRequestDto dto, CancellationToken ct)
        {
            try
            {
                var response = await _authService.LoginAsync(dto, ct);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { mensaje = ex.Message });
            }
        }

        /// <summary>
        /// Genera un token JWT con un TTL específico (en minutos o segundos) validando las credenciales en la BD.
        /// </summary>
        [HttpPost("token")]
        [AllowAnonymous]
        public async Task<ActionResult<JwtTokenDto>> GenerarToken([FromBody] LoginRequestDto dto, CancellationToken ct)
        {
            try
            {
                var tokenDto = await _authService.GenerarJwtAsync(dto, ct);
                return Ok(tokenDto);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { mensaje = ex.Message });
            }
        }

        /// <summary>
        /// Verifica el token JWT actual enviado en el encabezado Authorization: Bearer {token} y devuelve el TTL restante.
        /// </summary>
        [HttpGet("me")]
        [Authorize]
        public IActionResult ObtenerSesionActual()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            var username = User.FindFirstValue("username") ?? User.FindFirstValue(JwtRegisteredClaimNames.UniqueName);
            var nombre = User.FindFirstValue(ClaimTypes.Name);
            var correo = User.FindFirstValue(ClaimTypes.Email) ?? User.FindFirstValue(JwtRegisteredClaimNames.Email);
            var roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
            var expClaim = User.FindFirstValue(JwtRegisteredClaimNames.Exp);

            DateTime? expiracionUtc = null;
            int? ttlRestanteSegundos = null;
            if (long.TryParse(expClaim, out var expUnix))
            {
                expiracionUtc = DateTimeOffset.FromUnixTimeSeconds(expUnix).UtcDateTime;
                ttlRestanteSegundos = Math.Max(0, (int)Math.Round((expiracionUtc.Value - DateTime.UtcNow).TotalSeconds));
            }

            return Ok(new
            {
                id,
                username,
                nombre,
                correo,
                roles,
                expiracionUtc,
                ttlRestanteSegundos
            });
        }
    }
}
