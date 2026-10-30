using JWTAuthenticationSystem.API.Entities;

namespace JWTAuthenticationSystem.API.Interfaces.IRepositories
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken> AddRefreshToken(RefreshToken refreshToken);

        Task<RefreshToken?> GetByTokenHash(string tokenHash);

        Task UpdateRefreshToken(RefreshToken refreshToken);
        Task<bool> RotateRefreshToken(Guid oldTokenId, RefreshToken newToken);
    }
}