namespace Planora.Api.Services.Email;

public interface IEmailService
{
    System.Threading.Tasks.Task SendPasswordResetEmail(string toEmail, string resetToken);
}