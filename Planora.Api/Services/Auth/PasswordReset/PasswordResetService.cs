using Microsoft.AspNetCore.Identity;
using Planora.Api.Services.Email;
using Planora.DataAccess.Models.Auth;
using Planora.DTO.Auth;
using System.Threading.Tasks;

namespace Planora.Api.Services.Auth.PasswordReset;

public class PasswordResetService : IPasswordResetService
{
    private readonly UserManager<AuthUser> _userManager;
    private readonly IEmailService _emailService;

    public PasswordResetService(UserManager<AuthUser> userManager, IEmailService emailService)
    {
        _userManager = userManager;
        _emailService = emailService;
    }

    public async System.Threading.Tasks.Task RequestPasswordReset(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return;

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var link = $"https://planora/reset-password?email={email}&token={token}";

        //Should pass the link into the method
        await _emailService.SendPasswordResetEmailAsync(email, token);
    }
    
    public async Task<IdentityResult> ResetPassword(ResetPasswordDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null)
            return IdentityResult.Failed(new IdentityError { Description = "User not found" });

        return await _userManager.ResetPasswordAsync(user, dto.Token, dto.NewPassword);
    }
}
