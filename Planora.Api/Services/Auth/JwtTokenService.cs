using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Planora.DataAccess;
using Planora.DataAccess.Models;
using Planora.DataAccess.Models.Auth;

namespace Planora.Api.Services.Auth;

public class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _configuration;

    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    //Should return string 
    public string GenerateToken(AuthUser authUser)
    {
        var user = authUser.UserDb;
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        
        var claims = new []
        {
            new Claim(ClaimTypes.NameIdentifier, authUser.Id),
            new Claim("ApplicationUserId", user.Id.ToString()),
            new Claim(ClaimTypes.Role, authUser.Role),
            new Claim(ClaimTypes.Email, authUser.Email)
        };

        //Get Symmetric key
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        
        //Generate Token
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );
        
        //Return token
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}