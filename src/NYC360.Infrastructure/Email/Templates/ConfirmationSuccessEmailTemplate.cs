using NYC360.Application.Contracts.Emails;
using Microsoft.Extensions.Configuration;
using NYC360.Application.Models.Emails;

namespace NYC360.Infrastructure.Email.Templates;

public sealed class ConfirmationSuccessEmailTemplate(IConfiguration config) : IEmailTemplate<WelcomeEmailModel>
{
    public string Subject => $"Your {config["Info:Name"]} Email is Confirmed!";

    public string GenerateBody(WelcomeEmailModel model)
    {
        return $"""
                <div style="font-family: Arial, sans-serif; max-width: 600px; margin: auto; border: 1px solid #e0e0e0; border-radius: 8px; overflow: hidden;">
                    <div style="background-color: #4A90E2; color: #ffffff; padding: 20px; text-align: center;">
                        <h2 style="margin:0;">Email Confirmed!</h2>
                    </div>
                    <div style="padding: 30px; color: #333333;">
                        <p>Hello <strong>{model.Email}</strong>,</p>
                        <p>Your email has been successfully confirmed. 🎉</p>
                        <p>You can now log in and enjoy all {config["Info:Name"]} features.</p>
                        <p style="margin-top: 40px;">Cheers,<br/><strong>{config["Info:Name"]} Team</strong></p>
                    </div>
                    <div style="background-color: #f9f9f9; color: #777777; text-align: center; padding: 15px; font-size: 12px;">
                        &copy; 2025 {config["Info:Name"]}. All rights reserved.
                    </div>
                </div>
                """;
    }
}