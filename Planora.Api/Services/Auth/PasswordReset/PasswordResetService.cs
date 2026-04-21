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

    public Task<AuthResultDto> ResetPassword()
    {
        throw new NotImplementedException();
    }

    public async System.Threading.Tasks.Task RequestPasswordReset(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return;

        // In a fuller implementation we would generate a reset token and include it in the email
        //_emailServiceMock.SendPasswordResetEmail();
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var link = $"https://planora/reset-password?email={email}&token={token}";

        await _emailService.SendPasswordResetEmail();
    }
}