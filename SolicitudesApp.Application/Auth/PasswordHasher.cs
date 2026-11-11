using System.Security.Cryptography;
using System.Text;

namespace SolicitudesApp.Application.Auth
{
    public static class PasswordHasher
    {
        private const string Salt = "SolicitudesApp_Salt_2026!";

        /// <summary>
        /// Genera el hash SHA-256 determinista (con salt de aplicación) de una contraseña en texto plano.
        /// </summary>
        public static string Hash(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return string.Empty;

            var bytes = Encoding.UTF8.GetBytes($"{Salt}:{password.Trim()}");
            var hashBytes = SHA256.HashData(bytes);
            return Convert.ToHexString(hashBytes).ToLowerInvariant();
        }

        /// <summary>
        /// Verifica si la contraseña proporcionada coincide con el PasswordHash almacenado en BD.
        /// Soporta hash con salt de aplicación, SHA-256 simple y texto plano (para registros insertados manualmente en BD).
        /// </summary>
        public static bool Verify(string password, string storedPasswordHash)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(storedPasswordHash))
                return false;

            // 1. Comparación con el hash oficial de la aplicación
            var computedHash = Hash(password);
            if (CryptographicOperations.FixedTimeEquals(
                Encoding.UTF8.GetBytes(computedHash),
                Encoding.UTF8.GetBytes(storedPasswordHash.ToLowerInvariant())))
            {
                return true;
            }

            // 2. Comparación con SHA-256 directo sin salt
            var rawSha256 = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(password.Trim()))).ToLowerInvariant();
            if (CryptographicOperations.FixedTimeEquals(
                Encoding.UTF8.GetBytes(rawSha256),
                Encoding.UTF8.GetBytes(storedPasswordHash.ToLowerInvariant())))
            {
                return true;
            }

            // 3. Fallback texto plano (por si se insertó manualmente en SQL Server)
            return string.Equals(password.Trim(), storedPasswordHash.Trim(), StringComparison.Ordinal);
        }
    }
}
