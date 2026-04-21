using Planora.DTO.Auth;

namespace Planora.Api.Services.Auth;

public interface IAuthService
{
    Task<AuthResultDto> LoginAsync(LoginRequestDto dto);
    Task<AuthResultDto> RequestResetPassword(ResetPasswordDto dto);
}