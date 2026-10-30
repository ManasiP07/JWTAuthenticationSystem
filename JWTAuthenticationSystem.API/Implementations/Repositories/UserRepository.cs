using JWTAuthenticationSystem.API.Common;
using JWTAuthenticationSystem.API.Data;
using JWTAuthenticationSystem.API.Entities;
using JWTAuthenticationSystem.API.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace JWTAuthenticationSystem.API.Implementations.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GlobalApiResponseModel> GetUserByEmail(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (user == null)
            {
                return new GlobalApiResponseModel
                {
                    Data = null,
                    IsStatusCodeSuccessful = false,
                    StatusCode = 404,
                    Message = "User does not exist."
                };
            }

            return new GlobalApiResponseModel
            {
                Data = user,
                IsStatusCodeSuccessful = true,
                StatusCode = 200,
                Message = "User retrieved successfully."
            };
        }

        public async Task<GlobalApiResponseModel> GetUserById(Guid id)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                return new GlobalApiResponseModel
                {
                    Data = null,
                    IsStatusCodeSuccessful = false,
                    StatusCode = 404,
                    Message = "User not found."
                };
            }

            return new GlobalApiResponseModel
            {
                Data = user,
                IsStatusCodeSuccessful = true,
                StatusCode = 200,
                Message = "User retrieved successfully."
            };
        }

        public async Task<GlobalApiResponseModel> AddUser(User user)
        {
            await _context.Users.AddAsync(user);

            await _context.SaveChangesAsync();

            return new GlobalApiResponseModel
            {
                Data = user,
                IsStatusCodeSuccessful = true,
                StatusCode = 201,
                Message = "User added successfully."
            };
        }
    }
}