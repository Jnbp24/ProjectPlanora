namespace Planora.Api.Services.Email;

public interface IEmailService
{
    System.Threading.Tasks.Task SendPasswordResetEmail();
}