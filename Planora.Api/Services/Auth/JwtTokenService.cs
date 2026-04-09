using System.Security.Claims;
using Planora.DataAccess;
using Planora.DataAccess.Models;
using Planora.DataAccess.Models.Auth;

namespace Planora.Api.Services.Auth;

public class JwtTokenService
{
    private readonly IConfiguration _configuration;

    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    //Should return string 
    public string GenerateToken(AuthUser authUser, UserDB user)
    {
        // var claims = new []
        // {
        //     new Claim(ClaimTypes.NameIdentifier, user.Id),
        //     new Claim("UserId", user.Id.ToString()),
        //     new Claim(ClaimTypes.Role, user.GetRole()),
        //     new Claim(ClaimTypes.Email, authUser.Email)
        // };

        //Get Symmetric key
        
        //Generate Token
        
        //Return token
        return "";
    }
}