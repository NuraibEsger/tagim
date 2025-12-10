using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Tagim.Application.Interfaces;
using Tagim.Application.Models;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Tagim.Infrastructure.Services;

public class EmailService(IOptions<EmailSettings> emailSettings) : IEmailService
{
    private readonly EmailSettings _emailSettings = emailSettings.Value;

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = subject;

        var builder = new BodyBuilder();
        builder.HtmlBody = body;
        email.Body = builder.ToMessageBody();

        using var smtp = new SmtpClient();
        
        // To pass SSL certificate verification (for development environment)
        smtp.CheckCertificateRevocation = false;
        
        await smtp.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_emailSettings.SenderEmail, _emailSettings.Password);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}