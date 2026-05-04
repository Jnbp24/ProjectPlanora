using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using Planora.Api.Services.Auth;
using Planora.Api.Services.Auth.JwtToken;
using Planora.DataAccess;
using Planora.DataAccess.Models;
using Planora.DataAccess.Models.Auth;

namespace Api.UnitTests;


public class JwtTokenServiceTests
{
    private readonly JwtTokenService _jwtTokenService;
    private readonly Mock<UserManager<AuthUser>> _userManagerMock;

    public JwtTokenServiceTests()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                { "Jwt:Key", "this-is-a-test-secret-key-min-32-chars" },
                { "Jwt:Issuer", "test-issuer" },
                { "Jwt:Audience", "test-audience" }
            })
            .Build();
        
        _userManagerMock = new Mock<UserManager<AuthUser>>(
            Mock.Of<IUserStore<AuthUser>>(),
            null, null, null, null, null, null, null, null
        );

        _jwtTokenService = new JwtTokenService(configuration, _userManagerMock.Object);
    }
    
    [Fact]
    public async Task GenerateToken_ThrowException_OnUserEntityNull()
    {
        // Arrange
        var authUser = new AuthUser { Id = "auth-123", Email = "user@email.com", UserDb = null};

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _jwtTokenService.GenerateToken(authUser));
    }
    
    [Fact]
    public async Task GenerateToken_ValidInput_ReturnsToken()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var appUser = new UserDB { UserId = guid};
        var authUser = new AuthUser { Id = "auth-123", Email = "user@email.com", UserDb = appUser};
        
        var roles = new List<string> { "Tovholder" };
        _userManagerMock.Setup(x => x.GetRolesAsync(authUser))
            .ReturnsAsync(roles);

        // Act
        var token = await _jwtTokenService.GenerateToken(authUser);

        // Assert
        Assert.NotNull(token);
        Assert.NotEmpty(token);
    }
    
    [Fact]
    public async Task GenerateToken_ValidInput_ContainsCorrectClaims()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var appUser = new UserDB { UserId = guid};
        var authUser = new AuthUser { Id = "auth-123", Email = "user@email.com", UserDb = appUser};
        
        var roles = new List<string> { "Tovholder" };
        _userManagerMock.Setup(x => x.GetRolesAsync(authUser))
            .ReturnsAsync(roles);

        // Act
        var token = await _jwtTokenService.GenerateToken(authUser);

        // decode the token to inspect claims
        var handler = new JwtSecurityTokenHandler();
        var decoded = handler.ReadJwtToken(token);

        // Assert
        Assert.Equal("user@email.com", decoded.Claims.First(c => c.Type == ClaimTypes.Email).Value);
        Assert.Equal("Tovholder", decoded.Claims.First(c => c.Type == ClaimTypes.Role).Value);
        Assert.Equal("auth-123", decoded.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
        Assert.Equal(guid.ToString(), decoded.Claims.First(c => c.Type == "ApplicationUserId").Value);
    }
    
    [Fact]
    public async Task GenerateToken_ValidInput_TokenIsNotExpired()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var appUser = new UserDB { UserId = guid};
        var authUser = new AuthUser { Id = "auth-123", Email = "user@email.com",  UserDb = appUser};
        
        var roles = new List<string> { "Tovholder" };
        _userManagerMock.Setup(x => x.GetRolesAsync(authUser))
            .ReturnsAsync(roles);

        // Act
        var token = await _jwtTokenService.GenerateToken(authUser);

        var handler = new JwtSecurityTokenHandler();
        var decoded = handler.ReadJwtToken(token);

        // Assert
        Assert.True(decoded.ValidTo > DateTime.UtcNow);
    }
    
    [Fact]
    public async Task GenerateToken_ValidInput_HasCorrectIssuerAndAudience()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var appUser = new UserDB { UserId = guid};
        var authUser = new AuthUser { Id = "auth-123", Email = "user@email.com", UserDb = appUser};
        
        var roles = new List<string> { "Tovholder" };
        _userManagerMock.Setup(x => x.GetRolesAsync(authUser))
            .ReturnsAsync(roles);
        
        // Act
        var token = await _jwtTokenService.GenerateToken(authUser);

        var handler = new JwtSecurityTokenHandler();
        var decoded = handler.ReadJwtToken(token);

        // Assert
        Assert.Equal("test-issuer", decoded.Issuer);
        Assert.Contains("test-audience", decoded.Audiences);
    }
}