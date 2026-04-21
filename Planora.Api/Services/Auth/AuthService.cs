using Microsoft.AspNetCore.Identity;
using Planora.DataAccess.Models.Auth;
using Planora.DTO.Auth;

namespace Planora.Api.Services.Auth;

public class AuthService : IAuthService
{
    private readonly UserManager<AuthUser> _userManager;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IPasswordResetService _passwordResetService;

    public AuthService(UserManager<AuthUser> userManager, IJwtTokenService jwtTokenService, IPasswordResetService passwordResetService)
    {
        _userManager = userManager;
        _jwtTokenService = jwtTokenService;
        _passwordResetService = passwordResetService;
    }

    public async Task<AuthResultDto> LoginAsync(LoginRequestDto dto)
    {
        var authUser = await _userManager.FindByEmailAsync(dto.Email);
        if (authUser == null)
            return new AuthResultDto{Success =  false, Error = "Invalid Credentials"};

        var validPassword = await _userManager.CheckPasswordAsync(authUser, dto.Password);
        if (!validPassword)
            return new AuthResultDto{Success =  false, Error  = "Invalid Credentials"};

        var token = await _jwtTokenService.GenerateToken(authUser);
        return new AuthResultDto{Success =  true, Token = token};
    }

    public async System.Threading.Tasks.Task RequestResetPassword(EmailDto dto)
    {
        await _passwordResetService.RequestPasswordReset(dto.Email);
    }

    public async Task<IdentityResult> ResetPassword(ResetPasswordDto dto)
    {
        return await _passwordResetService.ResetPassword(dto);
    }
}