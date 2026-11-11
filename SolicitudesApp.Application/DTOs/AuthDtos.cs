namespace SolicitudesApp.Application.DTOs
{
    public class LoginRequestDto
    {
        /// <summary>
        /// Usuario o correo electrónico (ej. "ana.torres" o "ana.torres@solicitudes.com").
        /// </summary>
        public string? Usuario { get; set; }

        /// <summary>
        /// Alias opcional para enviar el username.
        /// </summary>
        public string? Username { get; set; }

        /// <summary>
        /// Alias opcional para enviar el correo.
        /// </summary>
        public string? Correo { get; set; }

        /// <summary>
        /// Alias opcional para compatibilidad con el frontend Angular (email).
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Contraseña del usuario (ej. "123456").
        /// </summary>
        public string Password { get; set; } = "";

        /// <summary>
        /// Tiempo de vida opcional del JWT en minutos (si no se envía, usa el configurado en appsettings.json).
        /// </summary>
        public int? TtlMinutos { get; set; }

        /// <summary>
        /// Tiempo de vida opcional del JWT en segundos (tiene prioridad sobre TtlMinutos si se especifica).
        /// </summary>
        public int? TtlSegundos { get; set; }

        public string ObtenerIdentificador() =>
            (Usuario ?? Username ?? Correo ?? Email ?? string.Empty).Trim();
    }

    public record JwtTokenDto(
        string Token,
        string TokenType,
        int TtlSeconds,
        int TtlMinutes,
        DateTime FechaEmisionUtc,
        DateTime FechaExpiracionUtc
    );

    public record AuthResponseDto(
        int Id,
        string Username,
        string Nombre,
        string Correo,
        string Email,
        string Rol,
        List<string> Roles,
        string Token,
        string TokenType,
        int TtlSeconds,
        int TtlMinutes,
        DateTime FechaEmisionUtc,
        DateTime Expiracion
    );
}
