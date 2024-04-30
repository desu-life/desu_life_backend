using Microsoft.AspNetCore.Identity.UI.Services;

namespace desu.life.Services;

internal class EmailSender : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        throw new NotImplementedException();
    }
}