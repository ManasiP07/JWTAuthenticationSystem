namespace JWTAuthenticationSystem.API.Interfaces.Services
{
    public interface IRefreshTokenService
    {
        string GenerateRefreshToken();
        string HashRefreshToken(string refreshToken);
    }
}