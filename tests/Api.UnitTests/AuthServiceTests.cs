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
            _jwtTokenServiceMock.Object
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
}