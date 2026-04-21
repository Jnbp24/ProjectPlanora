using Microsoft.AspNetCore.Identity;
using Planora.Api.Services.Email;
using Planora.DataAccess.Models.Auth;
using Planora.DTO.Auth;

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

    public Task<AuthResultDto> RequestPasswordReset(string email)
    {
        throw new NotImplementedException();
    }
}