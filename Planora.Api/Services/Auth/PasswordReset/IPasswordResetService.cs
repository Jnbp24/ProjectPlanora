using Microsoft.AspNetCore.Identity;
using Planora.DTO.Auth;

namespace Planora.Api.Services.Auth;

public interface IPasswordResetService
{
    Task<IdentityResult> ResetPassword(ResetPasswordDto dto);
    System.Threading.Tasks.Task RequestPasswordReset(string email);
}