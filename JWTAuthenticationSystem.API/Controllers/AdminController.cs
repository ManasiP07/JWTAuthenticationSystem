using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JWTAuthenticationSystem.API.Controllers
{
        [ApiController]
        [Route("api/[controller]")]
        [Authorize(Policy = "AdminOnly")]
        public class AdminController : ControllerBase
        {
            [HttpPost("dashboard")]
            public IActionResult GetAdminDashboard()
            {
                return Ok(new
                {
                    Message = "Welcome Admin! You have access to this API."
                });
            }
        }
}
