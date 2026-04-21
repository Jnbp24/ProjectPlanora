using Microsoft.AspNetCore.Identity;
using Planora.Api.Services.Email;
using Planora.DataAccess.Models.Auth;
using Planora.DTO.Auth;
using System.Threading.Tasks;

namespace Planora.Api.Services.Auth.PasswordReset;

public class PasswordResetService : IPasswordResetService
{
    private readonly UserManager<AuthUser> _userManager;
    private readonly IEmailService _emailServiceMock;

    public PasswordResetService(UserManager<AuthUser> userManager, IEmailService emailService)
    {
        _userManager = userManager;
        _emailServiceMock = emailService;
    }

    public Task<AuthResultDto> ResetPassword()
    {
        throw new NotImplementedException();
    }

    public async Task<AuthResultDto> RequestPasswordReset(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            // Nothing to do if user is unknown; do not send email
            return new AuthResultDto { Success = true};
        }

        // In a fuller implementation we would generate a reset token and include it in the email
        //_emailServiceMock.SendPasswordResetEmail();

        return new AuthResultDto { Success = true };
    }
}