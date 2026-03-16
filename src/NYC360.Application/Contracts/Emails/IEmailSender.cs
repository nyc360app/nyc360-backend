namespace NYC360.Application.Contracts.Emails;

public interface IEmailSender
{
    Task SendAsync(string to, string subject, string htmlBody, CancellationToken ct = default);
    Task SendSupportAsync(string to, string subject, string htmlBody, CancellationToken ct = default);
}