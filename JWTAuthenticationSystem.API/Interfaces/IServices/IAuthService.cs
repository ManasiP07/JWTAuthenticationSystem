using JWTAuthenticationSystem.API.Common;
using JWTAuthenticationSystem.API.DTOs;
using JWTAuthenticationSystem.API.DTOs.RequestModels;


namespace JWTAuthenticationSystem.API.Interfaces.IServices
{
    public interface IAuthService
    {
        Task<GlobalApiResponseModel> Register(RegisterRequestDTO model);
        Task<GlobalApiResponseModel> Login(LoginRequestDTO model);
        Task<GlobalApiResponseModel> RefreshToken(RefreshTokenRequestDTO model);
    }
}
