namespace Planora.Api.Services.Email;

public interface IEmailService
{
    System.Threading.Tasks.Task SendPasswordResetEmailAsync(string toEmail, string resetToken);
    System.Threading.Tasks.Task SendSignUpEmailAsync(string toEmail, string password);

}