using System.Net;
using System.Net.Mail;
using Eticaret.Models;
using Microsoft.Extensions.Options;

namespace Eticaret.Services;

public class MailService
{
    private readonly SmtpSettings _smtpSettings;

    public MailService(IOptions<SmtpSettings> smtpSettings)
    {
        _smtpSettings = smtpSettings.Value;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        using var smtp = new SmtpClient(_smtpSettings.Host)
        {
            Port = _smtpSettings.Port,
            Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
            EnableSsl = _smtpSettings.EnableSsl
        };

        var mail = new MailMessage
        {
            From = new MailAddress(_smtpSettings.SenderEmail, _smtpSettings.SenderName),
            Subject = subject,
            Body = message,
            IsBodyHtml = true
        };

        mail.To.Add(toEmail);

        await smtp.SendMailAsync(mail);
    }
}
