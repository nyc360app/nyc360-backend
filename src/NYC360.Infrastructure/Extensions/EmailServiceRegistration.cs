using Microsoft.Extensions.DependencyInjection;
using NYC360.Application.Contracts.Emails;
using NYC360.Application.Common.Settings;
using Microsoft.Extensions.Configuration;
using NYC360.Application.Models.Emails;
using NYC360.Infrastructure.Email;
using NYC360.Infrastructure.Email.Templates;

namespace NYC360.Infrastructure.Extensions;

public static class EmailServiceRegistration
{
    public static void RegisterEmailServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EmailSettings>(configuration.GetSection("Email"));
        services.AddScoped<IEmailSender, MailKitEmailSender>();
        services.AddScoped<IEmailService, EmailService>();
    }
}