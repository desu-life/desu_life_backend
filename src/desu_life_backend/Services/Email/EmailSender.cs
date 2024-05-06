#nullable disable

using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;

namespace desu.life.Services.Email;

public class EmailSender : IEmailSender
{
    private readonly string _sender;
    private readonly SmtpClient _smtpClient;

    public EmailSender(string host, int port, string username, string password, string secure, string from)
    {
        _sender = from;

        var secureSocketOptions = secure.ToLower() switch
        {
            "none" => SecureSocketOptions.None,
            "ssl" => SecureSocketOptions.SslOnConnect,
            "starttls" => SecureSocketOptions.StartTls,
            _ => throw new ArgumentException(
                $"Email service secure option {secure} is not supported now. (appsettings.json: Email.Secure)")
        };

        _smtpClient = new SmtpClient();
        _smtpClient.Connect(host, port, secureSocketOptions);
        _smtpClient.Authenticate(username, password);
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var mailMessage = new MimeMessage();
        mailMessage.From.Add(new MailboxAddress(_sender, _sender));
        mailMessage.To.Add(new MailboxAddress(email, email));
        mailMessage.Subject = subject;
        mailMessage.Body = new TextPart("html") { Text = htmlMessage };

        await _smtpClient.SendAsync(mailMessage);
    }
}