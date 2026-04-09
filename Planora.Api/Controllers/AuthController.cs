using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Planora.Api.Services.Auth;
using Planora.DTO.Auth;

namespace Planora.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] LoginRequestDto dto)
        {
            var result = await _authService.LoginAsync(dto);

            if (!result.Success)
            {
                return Unauthorized(result.Error);
            }
            
            return Ok(result.Token);
        }
    }
}
