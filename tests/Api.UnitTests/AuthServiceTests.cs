using Microsoft.AspNetCore.Identity;
using Moq;
using Planora.Api.Services.Auth;
using Planora.DataAccess.Models.Auth;
using Planora.DTO.Auth;

namespace Api.UnitTests;

public class AuthServiceTests
{
    private readonly Mock<UserManager<AuthUser>> _userManagerMock;
    private readonly Mock<IJwtTokenService> _jwtTokenServiceMock;
    private readonly AuthService _authService;
    
    public AuthServiceTests()
    {
        // UserManager requires a special setup to mock
        _userManagerMock = new Mock<UserManager<AuthUser>>(
            Mock.Of<IUserStore<AuthUser>>(),
            null, null, null, null, null, null, null, null
        );

        _jwtTokenServiceMock = new Mock<IJwtTokenService>();

        _authService = new AuthService(
            _userManagerMock.Object,
            _jwtTokenServiceMock.Object,
            null
        );
    }
    
    [Fact]
    public async Task LoginAsync_UserNotFound_ReturnsFailure()
    {
        //Arrange
        _userManagerMock
            .Setup(x => x.FindByEmailAsync("wrong@email.com"))
            .ReturnsAsync((AuthUser)null);
        
        var dto = new LoginRequestDto("wrong@email.com", "pass" );
        
        //Act
        var result = await _authService.LoginAsync(dto);
        
        //Assert
        Assert.False(result.Success);
        Assert.Equal("Invalid Credentials", result.Error);
    }
    
    [Fact]
    public async Task LoginAsync_WrongPassword_ReturnsFailure()
    {
        // Arrange
        var authUser = new AuthUser { Email = "user@email.com", UserDb = null};

        _userManagerMock
            .Setup(x => x.FindByEmailAsync("user@email.com"))
            .ReturnsAsync(authUser);

        _userManagerMock
            .Setup(x => x.CheckPasswordAsync(authUser, "wrongpassword"))
            .ReturnsAsync(false);

        var dto = new LoginRequestDto("user@email.com", "wrongpassword");

        // Act
        var result = await _authService.LoginAsync(dto);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Invalid Credentials", result.Error);
    }
    
    [Fact]
    public async Task LoginAsync_ValidCredentials_ReturnsToken()
    {
        // Arrange
        var authUser = new AuthUser { Email = "user@email.com", UserDb = null };

        _userManagerMock
            .Setup(x => x.FindByEmailAsync("user@email.com"))
            .ReturnsAsync(authUser);

        _userManagerMock
            .Setup(x => x.CheckPasswordAsync(authUser, "correctpassword"))
            .ReturnsAsync(true);

        _jwtTokenServiceMock
            .Setup(x => x.GenerateToken(authUser))
            .ReturnsAsync("mocked.jwt.token");

        var dto = new LoginRequestDto("user@email.com", "correctpassword");

        // Act
        var result = await _authService.LoginAsync(dto);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("mocked.jwt.token", result.Token);
    }
}