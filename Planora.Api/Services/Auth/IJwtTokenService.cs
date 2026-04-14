using Planora.DataAccess;
using Planora.DataAccess.Models.Auth;

namespace Planora.Api.Services.Auth;

public interface IJwtTokenService
{
    string GenerateToken(AuthUser authUser, UserDB user);
}