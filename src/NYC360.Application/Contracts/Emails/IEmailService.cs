using NYC360.Application.Models.Emails;

namespace NYC360.Application.Contracts.Emails;

public interface IEmailService
{
    Task SendWelcomeEmailAsync(string email, WelcomeEmailModel model, CancellationToken ct = default);
    Task SendVerificationSuccessEmailAsync(string email, WelcomeEmailModel token, CancellationToken ct = default);
    Task SendResetPasswordEmailAsync(string email, ResetPasswordEmailModel model, CancellationToken ct = default);
    Task SendPasswordChangedEmailAsync(string email, PasswordChangedEmailModel model, CancellationToken ct = default);
    Task SendOtpEmailAsync(string email, OtpEmailModel model, CancellationToken ct = default);
    Task SendSupportTicketAsync(SupportTicketModel model, CancellationToken ct = default);
    Task SendTicketReplyAsync(string email, TicketReplyEmailModel model, CancellationToken ct = default);
    Task SendAdminTicketNotificationAsync(AdminTicketNotificationModel model, CancellationToken ct = default);
}