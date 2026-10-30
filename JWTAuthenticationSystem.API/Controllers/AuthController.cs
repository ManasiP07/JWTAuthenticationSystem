using JWTAuthenticationSystem.API.Common;
using JWTAuthenticationSystem.API.DTOs;
using JWTAuthenticationSystem.API.DTOs.RequestModels;
using JWTAuthenticationSystem.API.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JWTAuthenticationSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDTO model)
        {
            var response = await _authService.Register(model);

            return StatusCode(response.StatusCode, response);

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDTO model)
        {
            var response = await _authService.Login(model);

            return StatusCode(response.StatusCode, response);
        }

        [Authorize]
        [HttpPost("profile")]
        public IActionResult GetProfile()
        {
            return Ok(new
            {
                Message = "You are authenticated!",
                UserId = User.FindFirst(
                    System.Security.Claims.ClaimTypes.NameIdentifier)?.Value,

                Email = User.FindFirst(
                    System.Security.Claims.ClaimTypes.Email)?.Value,

                Role = User.FindFirst(
                    System.Security.Claims.ClaimTypes.Role)?.Value
            });
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDTO model)
        {
            var response = await _authService.RefreshToken(model);

            return StatusCode(response.StatusCode, response);
        }
    }
}