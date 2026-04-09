using Microsoft.AspNetCore.Identity;

namespace Planora.DataAccess.Models.Auth;

public class AuthUser : IdentityUser
{
    public Guid UserId { get; set; }
    public required UserDB UserDb { get; set; }
}