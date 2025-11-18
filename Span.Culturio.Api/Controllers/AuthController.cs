using Microsoft.AspNetCore.Mvc;
using Span.Culturio.Api.Models.DTOs;
using Span.Culturio.Api.Services.Interfaces;

namespace Span.Culturio.Api.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {
            var result = await _authService.RegisterAsync(dto);

            if (!result)
                return BadRequest(new { message = "Username or email already exists" });

            return Ok(new { message = "User registered successfully" });
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var token = await _authService.LoginAsync(dto);

            if (token == null)
                return BadRequest(new { message = "Invalid username or password" });

            return Ok(new { token });
        }
    }
}
