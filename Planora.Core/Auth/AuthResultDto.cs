namespace Planora.DTO.Auth;

public class AuthResultDto
{
    public bool Success { get; set; }
    public string Token { get; set; }
    public string Error { get; set; }
}