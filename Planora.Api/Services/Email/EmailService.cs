using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Net;

namespace Planora.Api.Services.Email;

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public async System.Threading.Tasks.Task SendPasswordResetEmailAsync(string toEmail, string resetToken)
    {
        var email = SetUpEmail(toEmail);
        email.Subject = "Reset your Planora password";
        var encodedToken = WebUtility.UrlEncode(resetToken);
        var resetLink = $"https://localhost:7127/reset_password.html?email={toEmail}&token={encodedToken}"; 
        
        email.Body = new TextPart("html")
        {
            Text = $@"
            <p>Click below to reset your password:</p>
            <a href='{resetLink}'>Reset Password</a>
            <p>If you didn't request this, ignore this email.</p>"
        };

        await SendEmailAsync(email);
    }

    public async System.Threading.Tasks.Task SendSignUpEmailAsync(string toEmail, string password)
    {
        var email = SetUpEmail(toEmail);
        email.Subject = "Sign up to Planora";
        var encodedToken = WebUtility.UrlEncode(password);
        var signUpLink = $"https://localhost:7127/password_change.html?email={toEmail}"; 
        
        email.Body = new TextPart("html")
        {
            Text = $@"
            <p>Click below to sign up to Planora:</p>
            <a href='{signUpLink}'>Sign up & change password</a>
            <p>Temporary password: {password} </p>"
        };

        await SendEmailAsync(email);
    }
    
    
    private MimeMessage SetUpEmail(string toEmail)
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress("Planora", _config["Email:Username"]));
        email.To.Add(MailboxAddress.Parse(toEmail));
        return email;
    }

    private async System.Threading.Tasks.Task SendEmailAsync(MimeMessage email)
    {
        using var smtp = new SmtpClient();

        await smtp.ConnectAsync(
            _config["Email:SmtpHost"],
            int.Parse(_config["Email:SmtpPort"] ?? "587"),
            SecureSocketOptions.StartTls
        );

        await smtp.AuthenticateAsync(
            _config["Email:Username"],
            _config["Email:Password"]
        );

        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}