using NYC360.Application.Contracts.Emails;
using NYC360.Application.Common.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace NYC360.Infrastructure.Email;

public class MailKitEmailSender(
    IOptions<EmailSettings> options,
    ILogger<MailKitEmailSender> logger)
    : IEmailSender
{
    private readonly EmailSettings _settings = options.Value;

    public async Task SendAsync(string to, string subject, string htmlBody, CancellationToken ct = default)
    {
        await SendEmailAsync(to, subject, htmlBody, "NoReply", ct);
    }

    public async Task SendSupportAsync(string to, string subject, string htmlBody, CancellationToken ct = default)
    {
        await SendEmailAsync(to, subject, htmlBody, "Support", ct);
    }

    private async Task SendEmailAsync(string to, string subject, string htmlBody, string senderKey = "NoReply", CancellationToken ct = default)
    {
        if (!_settings.Accounts.TryGetValue(senderKey, out var account))
            throw new ArgumentException($"Email account '{senderKey}' not configured.");

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(account.FromDisplayName, account.User));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;

        var bodyBuilder = new BodyBuilder { HtmlBody = htmlBody };
        message.Body = bodyBuilder.ToMessageBody();

        try
        {
            using var client = new SmtpClient();
            var secureOption = _settings.Port == 465 ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.StartTls;

            await client.ConnectAsync(_settings.Host, _settings.Port, secureOption, ct);
            await client.AuthenticateAsync(account.User, account.Password, ct);
            await client.SendAsync(message, ct);
            await client.DisconnectAsync(true, ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send email via {Sender} to {To}", account.User, to);
            throw;
        }
    }
}