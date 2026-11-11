using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SolicitudesApp.Application.DTOs;
using SolicitudesApp.Application.Services;
using SolicitudesApp.Domain.Entidades;

namespace SolicitudesApp.Application.Auth
{
    public class JwtService : IJwtService
    {
        private const string DefaultKey = "ClaveSecretaSuperSeguraParaFirmarTokensSolicitudesApp2026FullStack!";
        private const string DefaultIssuer = "SolicitudesApp.Api";
        private const string DefaultAudience = "SolicitudesApp.Client";
        private const int DefaultTtlMinutes = 60;

        private readonly IConfiguration _config;

        public JwtService(IConfiguration config)
        {
            _config = config;
        }

        public JwtTokenDto GenerarToken(Usuario usuario, int? ttlMinutos, int? ttlSegundos = null)
        {
            TimeSpan? ttl = null;
            if (ttlSegundos is > 0)
            {
                ttl = TimeSpan.FromSeconds(ttlSegundos.Value);
            }
            else if (ttlMinutos is > 0)
            {
                ttl = TimeSpan.FromMinutes(ttlMinutos.Value);
            }

            return GenerarToken(usuario, ttl);
        }

        public JwtTokenDto GenerarToken(Usuario usuario, TimeSpan? ttl = null)
        {
            var ttlEfectivo = ResolverTtl(ttl);
            var ahoraUtc = DateTime.UtcNow;
            var expiracionUtc = ahoraUtc.Add(ttlEfectivo);
            var ttlSeconds = (int)Math.Round(ttlEfectivo.TotalSeconds);
            var ttlMinutes = (int)Math.Ceiling(ttlEfectivo.TotalMinutes);

            var roles = usuario.Roles?.Select(r => r.Nombre).Where(n => !string.IsNullOrWhiteSpace(n)).ToList()
                        ?? [];
            if (roles.Count == 0)
            {
                roles.Add("Usuario");
            }

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                new(JwtRegisteredClaimNames.UniqueName, string.IsNullOrWhiteSpace(usuario.Username) ? usuario.Correo : usuario.Username),
                new(JwtRegisteredClaimNames.Email, usuario.Correo),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Iat, new DateTimeOffset(ahoraUtc).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                new(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new(ClaimTypes.Name, usuario.Nombre),
                new(ClaimTypes.Email, usuario.Correo),
                new("username", string.IsNullOrWhiteSpace(usuario.Username) ? usuario.Correo : usuario.Username),
                new("ttl_seconds", ttlSeconds.ToString(), ClaimValueTypes.Integer32)
            };

            foreach (var rol in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, rol));
            }

            var secretKey = _config["Jwt:Key"] ?? DefaultKey;
            var issuer = _config["Jwt:Issuer"] ?? DefaultIssuer;
            var audience = _config["Jwt:Audience"] ?? DefaultAudience;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                notBefore: ahoraUtc,
                expires: expiracionUtc,
                signingCredentials: credentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return new JwtTokenDto(
                Token: tokenString,
                TokenType: "Bearer",
                TtlSeconds: ttlSeconds,
                TtlMinutes: ttlMinutes,
                FechaEmisionUtc: ahoraUtc,
                FechaExpiracionUtc: expiracionUtc
            );
        }

        public ClaimsPrincipal? ValidarToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return null;

            var secretKey = _config["Jwt:Key"] ?? DefaultKey;
            var issuer = _config["Jwt:Issuer"] ?? DefaultIssuer;
            var audience = _config["Jwt:Audience"] ?? DefaultAudience;

            var handler = new JwtSecurityTokenHandler();
            try
            {
                var parameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    ClockSkew = TimeSpan.Zero // Respeta estrictamente el TTL configurado
                };

                return handler.ValidateToken(token, parameters, out _);
            }
            catch
            {
                return null;
            }
        }

        private TimeSpan ResolverTtl(TimeSpan? ttlExplicito)
        {
            if (ttlExplicito is { TotalSeconds: > 0 })
                return ttlExplicito.Value;

            if (int.TryParse(_config["Jwt:TtlSeconds"], out var cfgSeconds) && cfgSeconds > 0)
                return TimeSpan.FromSeconds(cfgSeconds);

            if (int.TryParse(_config["Jwt:TtlMinutes"], out var cfgMinutes) && cfgMinutes > 0)
                return TimeSpan.FromMinutes(cfgMinutes);

            return TimeSpan.FromMinutes(DefaultTtlMinutes);
        }
    }
}
