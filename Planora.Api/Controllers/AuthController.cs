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
	public async Task<ActionResult> ResetPassword(ResetPasswordDto dto)
	{
		var result = await _authService.ResetPassword(dto);
			
		if (!result.Succeeded)
		{
			return BadRequest(new { message = "Password reset failed. The link may have expired." });
		}

		return Ok(new { message = "Password reset successful." });
	}
	
	[HttpPost("request-reset")]
	public async Task<ActionResult> RequestPasswordReset([FromBody] EmailDto dto, [FromServices] IServiceScopeFactory scopeFactory)
	{
		_ = Task.Run(async () =>
		{
			await using var scope = scopeFactory.CreateAsyncScope();
			var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();
			await authService.RequestResetPassword(dto);
		});
		
		return Ok(new { message = "If that email is registered, a reset link has been sent."});
	}
	
}
