using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BusinessCentralApi.Services;
using BusinessCentralApi.Models;
using Microsoft.AspNetCore.Authorization;

namespace BusinessCentralApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authService;

        public AuthController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] AuthRequestDto model)
        {
            try
            {
                var user = await _authService.RegisterAsync(model);
                return Ok(new
                {
                    message = "Registration successful",
                    username = user.Username
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] AuthRequestDto model)
        {
            try
            {
                var authResponse = await _authService.AuthenticateAsync(model);
                return Ok(authResponse);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }
    }
}