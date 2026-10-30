using System.Security.Cryptography;
using System.Text;
using JWTAuthenticationSystem.API.Interfaces.Services;

namespace JWTAuthenticationSystem.API.Implementations.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        // Generate a cryptographically secure random token
        public string GenerateRefreshToken()
        {
            byte[] randomBytes = RandomNumberGenerator.GetBytes(64);

            return Convert.ToBase64String(randomBytes);
        }

        // Convert the refresh token into a SHA-256 hash
        public string HashRefreshToken(string refreshToken)
        {
            byte[] tokenBytes = Encoding.UTF8.GetBytes(refreshToken);

            byte[] hashBytes = SHA256.HashData(tokenBytes);

            return Convert.ToHexString(hashBytes);
        }
    }
}