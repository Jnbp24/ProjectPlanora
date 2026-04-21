using System.Runtime.InteropServices.JavaScript;
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

    public Task<AuthResultDto> RequestResetPassword(ResetPasswordDto dto)
    {
        _passwordResetService.RequestPasswordReset(dto.Email);
        //Generate Secure Token
        //Send reset email with link
        throw new NotImplementedException();
    }
}