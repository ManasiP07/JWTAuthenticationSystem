using JWTAuthenticationSystem.API.Common;
using JWTAuthenticationSystem.API.Entities;

namespace JWTAuthenticationSystem.API.Interfaces.IRepositories
{
    public interface IUserRepository
    {
        Task<GlobalApiResponseModel> GetUserByEmail(string email);

        Task<GlobalApiResponseModel> GetUserById(Guid id);

        Task<GlobalApiResponseModel> AddUser(User user);
    }
}
