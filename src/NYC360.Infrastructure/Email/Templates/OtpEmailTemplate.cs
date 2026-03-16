using NYC360.Application.Contracts.Emails;
using Microsoft.Extensions.Configuration;
using NYC360.Application.Models.Emails;

namespace NYC360.Infrastructure.Email.Templates;

public sealed class OtpEmailTemplate(IConfiguration config) : IEmailTemplate<OtpEmailModel>
{
    public string Subject => $"Your OTP Code from {config["Info:Name"]}!";

    public string GenerateBody(OtpEmailModel model)
    {
        return $"""
                <div style="font-family: Arial, sans-serif; max-width: 600px; margin: auto; border: 1px solid #e0e0e0; border-radius: 8px; overflow: hidden;">
                    <div style="background-color: #4A90E2; color: #ffffff; padding: 20px; text-align: center;">
                        <h2 style="margin:0;">NYC360 OTP Verification</h2>
                    </div>
                    <div style="padding: 30px; color: #333333;">
                        <p>Hello <strong>{model.Name}</strong>,</p>
                        <p>Use the following OTP code to complete your action:</p>
                        <p style="text-align:center; margin: 30px 0;">
                            <span style="font-size: 24px; font-weight: bold; background-color: #f0f0f0; padding: 10px 20px; border-radius: 5px;">{model.OtpCode}</span>
                        </p>
                        <p>This code is valid for <strong>10 minutes</strong>.</p>
                        <p>If you did not request this, please ignore this email.</p>
                        <p style="margin-top: 40px;">Thanks,<br/><strong>NYC360 Team</strong></p>
                    </div>
                    <div style="background-color: #f9f9f9; color: #777777; text-align: center; padding: 15px; font-size: 12px;">
                        &copy; 2025 NYC360. All rights reserved.
                    </div>
                </div>
                        
                """;
    }
}