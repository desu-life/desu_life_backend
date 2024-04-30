#nullable disable

using Microsoft.AspNetCore.Identity.UI.Services;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace desu.life.Services;

internal class EmailSender : IEmailSender
{

    private readonly string _smtpServer = ConfigurationManager.AppSettings["SmtpSettings.Host"];
    private readonly int _smtpPort = int.Parse(ConfigurationManager.AppSettings["SmtpSettingsPort"]);
    private readonly string _smtpUsername = ConfigurationManager.AppSettings["SmtpSettings.Username"];
    private readonly string _smtpPassword = ConfigurationManager.AppSettings["SmtpSettings.Password"];
    
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        Console.WriteLine($"{_smtpServer}");
        throw new NotImplementedException();
    }
}