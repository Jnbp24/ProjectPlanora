using Planora.DataAccess.Models.Auth;

namespace Planora.Api.Services.Auth;

public interface IJwtTokenService
{
    Task<string> GenerateToken(AuthUser authUser);
}