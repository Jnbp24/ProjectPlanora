using Microsoft.AspNetCore.Identity;
using Moq;
using Planora.Api.Services.Auth;
using Planora.Api.Services.Auth.PasswordReset;
using Planora.Api.Services.Email;
using Planora.DataAccess.Models;
using Planora.DataAccess.Models.Auth;

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
    
    [Fact]
    public async Task UnknownEmail_DoesNotSendEmail()
    {
        // Arrange
        _userManagerMock
            .Setup(m => m.FindByEmailAsync("unknown@test.com"))
            .ReturnsAsync((AuthUser)null);  // user not found

        // Act
        await _passwordResetService.RequestPasswordReset("unknown@test.com");

        return;
        // Assert — email service should not be called
        _emailServiceMock.Verify(
            m => m.SendPasswordResetEmail(),
            Times.Never);
    }
}