using BCrypt.Net;
using JWTAuthenticationSystem.API.Common;
using JWTAuthenticationSystem.API.DTOs;
using JWTAuthenticationSystem.API.DTOs.RequestModels;
using JWTAuthenticationSystem.API.DTOs.ResponseModels;
using JWTAuthenticationSystem.API.DTOs.Responses;
using JWTAuthenticationSystem.API.Entities;
using JWTAuthenticationSystem.API.Interfaces.IRepositories;
using JWTAuthenticationSystem.API.Interfaces.IServices;
using JWTAuthenticationSystem.API.Interfaces.Services;

namespace JWTAuthenticationSystem.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public AuthService(IUserRepository userRepository, IJwtService jwtService, IRefreshTokenService refreshTokenService, IRefreshTokenRepository refreshTokenRepository)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _refreshTokenService = refreshTokenService;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<GlobalApiResponseModel> Register(RegisterRequestDTO model)
        {
            // Check whether user already exists
            var existingUserResponse =
            await _userRepository.GetUserByEmail(model.Email);

            if (existingUserResponse.StatusCode == 200)
            {
                return new GlobalApiResponseModel
                {
                    Data = null,
                    IsStatusCodeSuccessful = false,
                    StatusCode = 409,
                    Message = "User with this email already exists."
                };
            }

            // Hash password
            var passwordHash =
                BCrypt.Net.BCrypt.HashPassword(model.Password);

            // Create User entity
            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PasswordHash = passwordHash,
                Role = "User",
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = null
            };

            // Save user
            var createdUserResponse =
                await _userRepository.AddUser(user);

            // Check repository response
            if (!createdUserResponse.IsStatusCodeSuccessful)
            {
                return createdUserResponse;
            }

            // Get User from GlobalApiResponseModel.Data
            var createdUser =
                createdUserResponse.Data as User;

            if (createdUser == null)
            {
                return new GlobalApiResponseModel
                {
                    Data = null,
                    IsStatusCodeSuccessful = false,
                    StatusCode = 500,
                    Message = "Unable to retrieve created user."
                };
            }

            // Create response DTO
            
            var responseData = new RegisterResponseDTO
            {
                UId = createdUser.Id,
                FirstName = createdUser.FirstName,
                LastName = createdUser.LastName,
                Email = createdUser.Email,
                Role = createdUser.Role
            };

            // Return final API response
            return new GlobalApiResponseModel
            {
                Data = responseData,
                IsStatusCodeSuccessful = true,
                StatusCode = 201,
                Message = "User registered successfully."
            };
        }

        /*
         public async Task<GlobalApiResponseModel> Login(LoginRequestDTO model)
           {
               var userResponse = await _userRepository.GetUserByEmail(model.Email);

               if (!userResponse.IsStatusCodeSuccessful ||
                   userResponse.Data is not User user)
               {
                   return new GlobalApiResponseModel
                   {
                       Data = null,
                       IsStatusCodeSuccessful = false,
                       StatusCode = 404,
                       Message = "User not found."
                   };
               }

               var isPasswordValid =
                   BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash);

               if (!isPasswordValid)
               {
                   return new GlobalApiResponseModel
                   {
                       Data = null,
                       IsStatusCodeSuccessful = false,
                       StatusCode = 401,
                       Message = "Invalid email or password."
                   };
               }


               // Generate JWT
               var token = _jwtService.GenerateToken(user);

               // Prepare response
               var responseData = new LoginResponseDTO
               {
                   UId = user.Id,
                   FirstName = user.FirstName,
                   LastName = user.LastName,
                   Email = user.Email,
                   Role = user.Role,
                   Token = token
               };


               return new GlobalApiResponseModel
               {
                   Data = responseData,
                   IsStatusCodeSuccessful = true,
                   StatusCode = 200,
                   Message = "Login successful."
               };
           }
        */
        public async Task<GlobalApiResponseModel> Login(LoginRequestDTO model)
        {
            // 1. Find user by email
            var userResponse = await _userRepository.GetUserByEmail(model.Email);

            if (!userResponse.IsStatusCodeSuccessful ||
                userResponse.Data is not User user)
            {
                return new GlobalApiResponseModel
                {
                    Data = null,
                    IsStatusCodeSuccessful = false,
                    StatusCode = 401,
                    Message = "Invalid email or password."
                };
            }

            // 2. Verify password
            var isPasswordValid =
                BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash);

            if (!isPasswordValid)
            {
                return new GlobalApiResponseModel
                {
                    Data = null,
                    IsStatusCodeSuccessful = false,
                    StatusCode = 401,
                    Message = "Invalid email or password."
                };
            }

            // 3. Check whether user is active
            if (!user.IsActive)
            {
                return new GlobalApiResponseModel
                {
                    Data = null,
                    IsStatusCodeSuccessful = false,
                    StatusCode = 403,
                    Message = "Your account is inactive."
                };
            }

            // 4. Generate JWT access token
            var accessToken = _jwtService.GenerateToken(user);

            // 5. Generate refresh token
            var refreshToken = _refreshTokenService.GenerateRefreshToken();

            // 6. Hash refresh token before storing
            var tokenHash = _refreshTokenService.HashRefreshToken(refreshToken);

            // 7. Set expiry date
            var currentDate = DateTime.UtcNow;
            var expiryDate = currentDate.AddDays(7);

            // 8. Prepare refresh token entity
            var refreshTokenEntity = new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                TokenHash = tokenHash,
                CreatedDate = currentDate,
                ExpiryDate = expiryDate,
                IsRevoked = false,
                RevokedDate = null
            };

            // 9. Save refresh token hash in SQL Server
            await _refreshTokenRepository.AddRefreshToken(refreshTokenEntity);

            // 10. Prepare login response
            var responseData = new LoginResponseDTO
            {
                UId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role,
                Token = accessToken,
                RefreshToken = refreshToken,
                RefreshTokenExpiryDate = expiryDate
            };

            return new GlobalApiResponseModel
            {
                Data = responseData,
                IsStatusCodeSuccessful = true,
                StatusCode = 200,
                Message = "Login successful."
            };
        }


        public async Task<GlobalApiResponseModel> RefreshToken(
            RefreshTokenRequestDTO model)
        {
            if (model == null ||
                string.IsNullOrWhiteSpace(model.RefreshToken))
            {
                return RefreshTokenError("Refresh token is required.");
            }

            // 1. Hash the refresh token received from the client
            var tokenHash =
                _refreshTokenService.HashRefreshToken(model.RefreshToken);

            // 2. Find the existing refresh token
            var existingToken =
                await _refreshTokenRepository.GetByTokenHash(tokenHash);

            var now = DateTime.UtcNow;

            if (existingToken == null ||
                existingToken.IsRevoked ||
                existingToken.ExpiryDate <= now)
            {
                return RefreshTokenError("Invalid or expired refresh token.");
            }

            // 3. Retrieve the associated user
            var userResponse =
                await _userRepository.GetUserById(existingToken.UserId);

            if (!userResponse.IsStatusCodeSuccessful ||
                userResponse.Data is not User user ||
                !user.IsActive)
            {
                return RefreshTokenError("Invalid or expired refresh token.");
            }

            // 4. Generate a new JWT access token
            var accessToken = _jwtService.GenerateToken(user);

            // 5. Generate and hash a new refresh token
            var newRefreshToken =
                _refreshTokenService.GenerateRefreshToken();

            var newTokenHash =
                _refreshTokenService.HashRefreshToken(newRefreshToken);

            var expiryDate = now.AddDays(7);

            var newTokenEntity = new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                TokenHash = newTokenHash,
                CreatedDate = now,
                ExpiryDate = expiryDate,
                IsRevoked = false,
                RevokedDate = null
            };

            // 6. Atomically revoke the old token and save the new one
            var rotated = await _refreshTokenRepository.RotateRefreshToken(
                existingToken.Id,
                newTokenEntity);

            if (!rotated)
            {
                return RefreshTokenError("Invalid or expired refresh token.");
            }

            // 7. Return the new tokens
            return new GlobalApiResponseModel
            {
                IsStatusCodeSuccessful = true,
                StatusCode = 200,
                Message = "Token refreshed successfully.",
                Data = new RefreshTokenResponseDTO
                {
                    AccessToken = accessToken,
                    RefreshToken = newRefreshToken,
                    RefreshTokenExpiryDate = expiryDate
                }
            };
        }


        private static GlobalApiResponseModel RefreshTokenError(string message)
        {
            return new GlobalApiResponseModel
            {
                Data = null,
                IsStatusCodeSuccessful = false,
                StatusCode = 401,
                Message = message
            };
        }
    }
}