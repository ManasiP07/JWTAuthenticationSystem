using JWTAuthenticationSystem.API.Data;
using JWTAuthenticationSystem.API.Entities;
using JWTAuthenticationSystem.API.Interfaces;
using JWTAuthenticationSystem.API.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace JWTAuthenticationSystem.API.Implementations.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly ApplicationDbContext _context;

        public RefreshTokenRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Save a new refresh token
        public async Task<RefreshToken> AddRefreshToken(
            RefreshToken refreshToken)
        {
            await _context.RefreshTokens.AddAsync(refreshToken);

            await _context.SaveChangesAsync();

            return refreshToken;
        }

        // 2. Find a refresh token by its hash
        public async Task<RefreshToken?> GetByTokenHash(
            string tokenHash)
        {
            return await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.TokenHash == tokenHash);
        }

        // 3. Update an existing refresh token
        public async Task UpdateRefreshToken(
            RefreshToken refreshToken)
        {
            _context.RefreshTokens.Update(refreshToken);

            await _context.SaveChangesAsync();
        }


        public async Task<bool> RotateRefreshToken(Guid oldTokenId, RefreshToken newToken)
        {
            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            var now = DateTime.UtcNow;

            var updatedRows = await _context.RefreshTokens
                .Where(rt => rt.Id == oldTokenId
                          && !rt.IsRevoked
                          && rt.ExpiryDate > now)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(rt => rt.IsRevoked, true)
                    .SetProperty(rt => rt.RevokedDate, now));

            if (updatedRows != 1)
            {
                await transaction.RollbackAsync();
                return false;
            }

            await _context.RefreshTokens.AddAsync(newToken);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return true;
        }
    }
}