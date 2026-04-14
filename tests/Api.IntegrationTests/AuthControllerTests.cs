namespace Api.IntegrationTests;

public class AuthControllerTests
{
    // private readonly HttpClient _client;
    //
    // public AuthControllerTests(WebApplicationFactory<Program> factory)
    // {
    //     _client = factory.CreateClient();
    // }
    //
    // [Fact]
    // public async Task Login_ValidCredentials_ReturnsToken()
    // {
    //     // Arrange
    //     var loginRequest = new LoginRequestDto
    //     {
    //         Email = "test@email.com",
    //         Password = "Test1234!"
    //     };
    //
    //     var content = new StringContent(
    //         JsonSerializer.Serialize(loginRequest),
    //         Encoding.UTF8,
    //         "application/json"
    //     );
    //
    //     // Act
    //     var response = await _client.PostAsync("/api/auth/login", content);
    //     var body = await response.Content.ReadAsStringAsync();
    //     var result = JsonSerializer.Deserialize<AuthResultDto>(body);
    //
    //     // Assert
    //     Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    //     Assert.NotNull(result.Token);
    //     Assert.NotEmpty(result.Token);
    // }
    //
    // [Fact]
    // public async Task Login_InvalidCredentials_Returns401()
    // {
    //     // Arrange
    //     var loginRequest = new LoginRequestDto
    //     {
    //         Email = "wrong@email.com",
    //         Password = "wrongpassword"
    //     };
    //
    //     var content = new StringContent(
    //         JsonSerializer.Serialize(loginRequest),
    //         Encoding.UTF8,
    //         "application/json"
    //     );
    //
    //     // Act
    //     var response = await _client.PostAsync("/api/auth/login", content);
    //
    //     // Assert
    //     Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    // }
}