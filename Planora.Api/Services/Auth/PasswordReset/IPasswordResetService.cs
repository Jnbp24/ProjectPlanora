using Planora.DTO.Auth;

namespace Planora.Api.Services.Auth;

public interface IPasswordResetService
{
    Task<AuthResultDto> ResetPassword();
    Task<AuthResultDto> RequestPasswordReset(string dto);
}