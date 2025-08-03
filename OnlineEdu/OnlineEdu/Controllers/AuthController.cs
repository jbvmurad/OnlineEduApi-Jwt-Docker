using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using OnlineEdu.DTOs.DTOs.AuthDTOs;
using Microsoft.AspNetCore.Mvc;
using OnlineEdu.Data.Abstract;

namespace OnlineEdu.API.Controllers
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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _authService.LoginAsync(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await _authService.RegisterAsync(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("change-role")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> ChangeRole([FromBody] ChangeRoleRequest request)
        {
            var result = await _authService.ChangeUserRoleAsync(request.Username, request.Role);

            if (!result)
                return BadRequest("Role could't be changed");

            return Ok(new { message = "Role changed successfully" });
        }

        [HttpGet("users")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _authService.GetAllUsersAsync();
            return Ok(users);
        }
    }

    public class ChangeRoleRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty; 
    }
}
