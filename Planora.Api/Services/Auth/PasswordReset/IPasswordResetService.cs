using Planora.DTO.Auth;

namespace Planora.Api.Services.Auth;

public interface IPasswordResetService
{
    Task<AuthResultDto> ResetPassword();
    System.Threading.Tasks.Task RequestPasswordReset(string dto);
}