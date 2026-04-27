using Microsoft.AspNetCore.Mvc;
using Planora.Api.Services.Auth;
using Planora.DTO.Auth;

namespace Planora.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
	private readonly IAuthService _authService;
	private readonly IPasswordService _passwordService;

	public AuthController(IAuthService authService, IPasswordService passwordService)
	{
		_authService = authService;
		_passwordService = passwordService;
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
		var result = await _passwordService.ResetPassword(dto);
			
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
			var passwordService = scope.ServiceProvider.GetRequiredService<IPasswordService>();
			await passwordService.RequestPasswordReset(dto.Email);
		});
		
		return Ok(new { message = "If that email is registered, a reset link has been sent."});
	}
	
	[HttpPost("password-change")]
    public async Task<ActionResult> PasswordChange([FromBody] PasswordChangeDto dto)
    {
	    var result = await _passwordService.ChangePassword(dto);
			
	    if (!result.Succeeded)
	    {
		    return BadRequest(new { errors = result.Errors.Select(e => e.Description) });
	    }

	    return Ok(new { message = "Password change successful." });
    }
}
