using Microsoft.AspNetCore.Mvc;
using RMS.Application.Interfaces;
using RMS.Application.DTOs;
using System.Threading.Tasks;

namespace RMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST: api/Auth/Login
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                // Send the credentials to the Chef. 
                // If they are correct, he hands back the VIP Pass (JWT).
                var response = await _authService.LoginAsync(loginDto);
                return Ok(response);
            }
            catch (System.Exception ex)
            {
                // If BCrypt fails or the EmployeeNo is wrong, return a 401 Unauthorized!
                return Unauthorized(ex.Message);
            }
        }
    }
}
