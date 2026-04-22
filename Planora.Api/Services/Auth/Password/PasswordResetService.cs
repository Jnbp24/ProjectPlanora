using Microsoft.AspNetCore.Identity;
using Planora.Api.Services.Email;
using Planora.DataAccess.Models.Auth;
using Planora.DTO.Auth;
using System.Threading.Tasks;

namespace Planora.Api.Services.Auth.Password;

public class PasswordService : IPasswordService
{
    private readonly UserManager<AuthUser> _userManager;
    private readonly IEmailService _emailService;

    public PasswordService(UserManager<AuthUser> userManager, IEmailService emailService)
    {
        _userManager = userManager;
        _emailService = emailService;
    }

    public async System.Threading.Tasks.Task RequestPasswordReset(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return;

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        //Should pass the link into the method
        await _emailService.SendPasswordResetEmail(email, token);
    }
    
    public async Task<IdentityResult> ResetPassword(ResetPasswordDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null)
            return IdentityResult.Failed(new IdentityError { Description = "User not found" });

        return await _userManager.ResetPasswordAsync(user, dto.Token, dto.NewPassword);
    }
    
    public async Task<IdentityResult> ChangePassword(PasswordChangeDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        
        if (user == null)
            return IdentityResult.Failed(new IdentityError { Description = "User not found" });
        
        return await _userManager.ChangePasswordAsync(user, dto.OldPassword, dto.NewPassword);
    }
}
