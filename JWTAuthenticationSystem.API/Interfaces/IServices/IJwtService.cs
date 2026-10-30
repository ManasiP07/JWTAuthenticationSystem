using JWTAuthenticationSystem.API.Entities;

namespace JWTAuthenticationSystem.API.Interfaces.IServices
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
