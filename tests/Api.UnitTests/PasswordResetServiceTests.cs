using Microsoft.AspNetCore.Identity;
using Moq;
using Planora.Api.Services.Auth;
using Planora.Api.Services.Auth.PasswordReset;
using Planora.Api.Services.Email;
using Planora.DataAccess.Models;
using Planora.DataAccess.Models.Auth;
using Planora.DTO.Auth;

namespace Api.UnitTests;

public class PasswordResetServiceTests
{
    private readonly Mock<UserManager<AuthUser>> _userManagerMock;
    private readonly Mock<IEmailService> _emailServiceMock;
    private readonly PasswordResetService _passwordResetService;

    public PasswordResetServiceTests()
    {
        // UserManager requires a IUserStore mock as its constructor argument
        var store = new Mock<IUserStore<AuthUser>>();
        _userManagerMock = new Mock<UserManager<AuthUser>>(
            store.Object, null, null, null, null, null, null, null, null);

        _emailServiceMock = new Mock<IEmailService>();

        _passwordResetService = new PasswordResetService(
            _userManagerMock.Object,
            _emailServiceMock.Object
        );

    }
    
    //Test for password reset request
    [Fact]
    public async Task UnknownEmail_DoesNotSendEmail()
    {
        // Arrange
        _userManagerMock
            .Setup(m => m.FindByEmailAsync("unknown@test.com"))
            .ReturnsAsync((AuthUser)null);  // user not found

        // Act
        await _passwordResetService.RequestPasswordReset("unknown@test.com");
        
        // Assert — email service should not be called
        _emailServiceMock.Verify(
            m => m.SendPasswordResetEmail(It.IsAny<string>(), It.IsAny<string>()),
            Times.Never);
    }
    
    [Fact]
    public async Task ForgotPassword_KnownEmail_SendsResetEmail()
    {
        // Arrange
        var user = new AuthUser{ Email = "user@test.com", UserDb = null};
        _userManagerMock
            .Setup(m => m.FindByEmailAsync("user@test.com"))
            .ReturnsAsync(user);
        _userManagerMock
            .Setup(m => m.GeneratePasswordResetTokenAsync(user))
            .ReturnsAsync("fake-token");

        // Act
        await _passwordResetService.RequestPasswordReset("user@test.com");
        
        // Assert — email must be sent exactly once to the right address
        _emailServiceMock.Verify(
            m => m.SendPasswordResetEmail("user@test.com", "fake-token"),
            Times.Once);
    }
    
    // Test for password reset

    [Fact]
    public async Task ResetPassword_InvalidToken_ReturnsFailure()
    {
        // Arrange
        var user = new AuthUser{ Email = "user@test.com", UserDb =  null};
        _userManagerMock
            .Setup(m => m.FindByEmailAsync("user@test.com"))
            .ReturnsAsync(user);
        _userManagerMock
            .Setup(m => m.ResetPasswordAsync(user, "bad-token", "NewPass123!"))
            .ReturnsAsync(IdentityResult.Failed(
                new IdentityError { Description = "Invalid token" }));

        // Act
        var dto = new ResetPasswordDto{Email = "user@test.com", Token = "bad-token", NewPassword =  "NewPass123!"};
        var result = await _passwordResetService.ResetPassword(dto);

        // Assert
        Assert.False(result.Succeeded);
    }

    [Fact]
    public async Task ResetPassword_ValidToken_ReturnsSuccess()
    {
        // Arrange
        var user = new AuthUser{ Email = "user@test.com", UserDb =  null};
        _userManagerMock
            .Setup(m => m.FindByEmailAsync("user@test.com"))
            .ReturnsAsync(user);
        _userManagerMock
            .Setup(m => m.ResetPasswordAsync(user, "valid-token", "NewPass123!"))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var dto = new ResetPasswordDto{Email = "user@test.com", Token = "valid-token", NewPassword =  "NewPass123!"};
        var result = await _passwordResetService.ResetPassword(dto);

        // Assert
        Assert.True(result.Succeeded);
    }

    [Fact]
    public async Task ResetPassword_UserNotFound_ReturnsFailure()
    {
        // Arrange
        _userManagerMock
            .Setup(m => m.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((AuthUser)null);

        // Act
        var dto = new ResetPasswordDto{Email = "user@test.com", Token = "valid-token", NewPassword =  "NewPass123!"};
        var result = await _passwordResetService.ResetPassword(dto);

        // Assert
        Assert.False(result.Succeeded);
    }
}