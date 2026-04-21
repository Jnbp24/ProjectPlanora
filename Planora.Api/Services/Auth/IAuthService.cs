using Microsoft.AspNetCore.Identity;
using Planora.DTO.Auth;

namespace Planora.Api.Services.Auth;

public interface IAuthService
{
    Task<AuthResultDto> LoginAsync(LoginRequestDto dto);
    System.Threading.Tasks.Task RequestResetPassword(EmailDto dto);
    Task<IdentityResult> ResetPassword(ResetPasswordDto dto);
}