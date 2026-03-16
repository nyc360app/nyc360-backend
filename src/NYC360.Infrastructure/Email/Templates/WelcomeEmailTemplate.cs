using NYC360.Application.Contracts.Emails;
using Microsoft.Extensions.Configuration;
using NYC360.Application.Models.Emails;

namespace NYC360.Infrastructure.Email.Templates;

public sealed class WelcomeEmailTemplate(IConfiguration config) : IEmailTemplate<WelcomeEmailModel>
{
    public string Subject => $"Welcome to {config["Info:Name"]}!";

    public string GenerateBody(WelcomeEmailModel model)
    {
        var tokenPart = !string.IsNullOrEmpty(model.Token) 
            ? $"""
               <p>To get started, please confirm your email address by clicking the button below:</p>
               <p style="text-align:center; margin: 30px 0;">
                   <a href="{config["Info:FrontendUrl"]}/auth/confirm-email?email={model.Email}&token={model.Token}" 
                      style="display:inline-block; padding: 12px 25px; background-color: #4A90E2; color: #ffffff; text-decoration: none; border-radius: 5px; font-weight: bold;">
                       Confirm Email
                   </a>
               </p>
               """
            : "";
        return $"""
                <div style="font-family: Arial, sans-serif; max-width: 600px; margin: auto; border: 1px solid #e0e0e0; border-radius: 8px; overflow: hidden;">
                    <div style="background-color: #4A90E2; color: #ffffff; padding: 20px; text-align: center;">
                        <h2 style="margin:0;">Welcome to {config["Info:Name"]}, {model.FullName}!</h2>
                    </div>
                    <div style="padding: 30px; color: #333333;">
                        <p>Thank you for registering with {config["Info:Name"]}.</p>
                        {tokenPart}
                        <p>If you did not register, please ignore this email.</p>
                        <p style="margin-top: 40px;">Cheers,<br/><strong>{config["Info:Name"]} Team</strong></p>
                    </div>
                    <div style="background-color: #f9f9f9; color: #777777; text-align: center; padding: 15px; font-size: 12px;">
                        &copy; 2025 {config["Info:Name"]}. All rights reserved.
                    </div>
                </div>
                """;
    }
}