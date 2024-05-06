#nullable disable

using System.Collections.Immutable;
using System.Net;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using MimeKit;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace desu.life.Services;

public class EmailSender : IEmailSender
{
    private readonly SmtpClient _smtpClient;
    private readonly string _sender;
    
    public EmailSender(string host, int port, string username, string password, bool enableSsl, string from)
    {
        _sender = from;

        _smtpClient = new SmtpClient();
        _smtpClient.Connect(host, port, enableSsl ? MailKit.Security.SecureSocketOptions.StartTls : MailKit.Security.SecureSocketOptions.Auto);
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