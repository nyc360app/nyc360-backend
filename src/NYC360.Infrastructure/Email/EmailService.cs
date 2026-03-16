using NYC360.Infrastructure.Email.Templates;
using NYC360.Application.Contracts.Emails;
using Microsoft.Extensions.Configuration;
using NYC360.Application.Models.Emails;

namespace NYC360.Infrastructure.Email;

public sealed class EmailService(
    IConfiguration config, 
    IEmailSender sender) 
    : IEmailService
{
    public async Task SendWelcomeEmailAsync(string email, WelcomeEmailModel model, CancellationToken ct = default)
    {
        var template = new WelcomeEmailTemplate(config);
        await sender.SendAsync(email, template.Subject, template.GenerateBody(model), ct);
    }

    public async Task SendVerificationSuccessEmailAsync(string email, WelcomeEmailModel model, CancellationToken ct = default)
    {
        var template = new ConfirmationSuccessEmailTemplate(config);
        await sender.SendAsync(email, template.Subject, template.GenerateBody(model), ct);
    }

    public async Task SendResetPasswordEmailAsync(string email, ResetPasswordEmailModel model, CancellationToken ct = default)
    {
        var template = new PasswordResetEmailTemplate(config);
        await sender.SendAsync(email, template.Subject, template.GenerateBody(model), ct);
    }

    public async Task SendPasswordChangedEmailAsync(string email, PasswordChangedEmailModel model, CancellationToken ct = default)
    {
        var template = new PasswordChangedEmailTemplate(config);
        await sender.SendAsync(email, template.Subject, template.GenerateBody(model), ct);
    }

    public async Task SendOtpEmailAsync(string email, OtpEmailModel model, CancellationToken ct = default)
    {
        var template = new OtpEmailTemplate(config);
        await sender.SendAsync(email, template.Subject, template.GenerateBody(model), ct);
    }

    public async Task SendSupportTicketAsync(SupportTicketModel model, CancellationToken ct = default)
    {
        var template = new SupportTicketTemplate(config);
        await sender.SendSupportAsync(model.Email, template.Subject, template.GenerateBody(model), ct);
    }
    
    public async Task SendTicketReplyAsync(string email, TicketReplyEmailModel model, CancellationToken ct = default)
    {
        var template = new TicketReplyTemplate(config);
        await sender.SendSupportAsync(email, template.Subject, template.GenerateBody(model), ct);
    }

    public Task SendAdminTicketNotificationAsync(AdminTicketNotificationModel model, CancellationToken ct = default)
    {
        // 1. Initialize the specific admin template
        // var template = new AdminTicketNotificationTemplate(config);
        //
        // // 2. Use the sender to dispatch the email
        // // We use "Support" as the senderKey to use the support@nyc360.app credentials
        // await sender.SendAsync(
        //     adminEmail, 
        //     template.Subject, 
        //     template.GenerateBody(model), 
        //     "Support", 
        //     ct);
        return Task.CompletedTask;
    }
}