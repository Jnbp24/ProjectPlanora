using Microsoft.AspNetCore.Mvc;
using Planora.Api.Services.Auth;
using Planora.DTO.Auth;

namespace Planora.Api.Controllers;

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
	public async Task<ActionResult> Login([FromBody] LoginRequestDto dto)
	{
		var result = await _authService.LoginAsync(dto);

		if (!result.Success)
		{
			return Unauthorized(new { error = result.Error });
		}

			return Ok(new { token = result.Token });
		}

		[HttpPost("reset")]
		public async Task<ActionResult> ResetPassword()
		{
			throw new NotImplementedException();
		}
		
		[HttpPost("request-reset")]
		public async Task<ActionResult> RequestPasswordReset([FromBody] ResetPasswordDto dto)
		{
			await _authService.RequestResetPassword(dto);
			return Ok();
		}
	}
}
