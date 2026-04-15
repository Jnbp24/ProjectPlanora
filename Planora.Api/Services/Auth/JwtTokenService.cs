using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Planora.DataAccess;
using Planora.DataAccess.Models;
using Planora.DataAccess.Models.Auth;

namespace Planora.Api.Services.Auth;

public class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<AuthUser> _userManager;

    public JwtTokenService(IConfiguration configuration, UserManager<AuthUser> userManager)
    {
        _configuration = configuration;
        _userManager = userManager;
    }

    //Should return string 
    public async Task<string> GenerateToken(AuthUser authUser)
    {
        var user = authUser.UserDb;
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        
        var roles = await _userManager.GetRolesAsync(authUser);
        
        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, authUser.Id),
            new Claim("ApplicationUserId", user.UserId.ToString()),
            new Claim(ClaimTypes.Email, authUser.Email)
        };
        
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

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