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

    public async System.Threading.Tasks.Task SendPasswordResetEmail(string toEmail, string resetToken)
    {
        var encodedToken = WebUtility.UrlEncode(resetToken);
        var resetLink = $"https://localhost:7127/reset_password.html?email={toEmail}&token={encodedToken}";

        var email = new MimeMessage();
        email.From.Add(new MailboxAddress("Planora", _config["Email:Username"]));
        email.To.Add(MailboxAddress.Parse(toEmail));
        email.Subject = "Reset your Planora password";

        email.Body = new TextPart("html")
        {
            Text = $@"
            <p>Click below to reset your password:</p>
            <a href='{resetLink}'>Reset Password</a>
            <p>If you didn't request this, ignore this email.</p>"
        };

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