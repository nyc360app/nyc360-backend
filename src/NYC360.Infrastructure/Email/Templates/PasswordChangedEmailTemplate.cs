using NYC360.Application.Contracts.Emails;
using Microsoft.Extensions.Configuration;
using NYC360.Application.Models.Emails;

namespace NYC360.Infrastructure.Email.Templates;

public sealed class PasswordChangedEmailTemplate(IConfiguration config) : IEmailTemplate<PasswordChangedEmailModel>
{
    public string Subject => $"Your Password Changed on {config["Info:Name"]}!";

    public string GenerateBody(PasswordChangedEmailModel model)
    {
        return $"""
                <div style="font-family: Arial, sans-serif; padding: 24px; background-color: #f5f5f5;">
                    <div style="max-width: 600px; margin: auto; background: #ffffff; padding: 32px; border-radius: 8px;">

                        <h2 style="color: #333;">Password Changed Successfully</h2>
                        <p style="font-size: 15px; color: #555;">
                            Hi {model.FullName},
                        </p>

                        <p style="font-size: 15px; color: #555;">
                            This is a confirmation that the password for your {config["Info:Name"]} account 
                            <strong>{model.Email}</strong> has just been changed.
                        </p>

                        <p style="font-size: 15px; color: #555;">
                            If you made this change, no further action is required.
                        </p>

                        <p style="font-size: 15px; color: #b00000; font-weight: bold;">
                            If you did NOT change your password, please reset it immediately.
                        </p>

                        <a href="{config["Info:FrontendUrl"]}/auth/reset-password" 
                           style="display: inline-block; padding: 12px 18px; background-color: #007bff; color: white; 
                                  text-decoration: none; border-radius: 6px; margin-top: 12px;">
                            Reset Password
                        </a>

                        <p style="font-size: 13px; color: #999; margin-top: 24px;">
                            If you have any concerns, contact NYC360 support.
                        </p>
                    </div>
                </div>
        """;
    }
}