using System.Net;
using Microsoft.Extensions.Configuration;
using NYC360.Application.Contracts.Emails;
using NYC360.Application.Models.Emails;

namespace NYC360.Infrastructure.Email.Templates;

public class PasswordResetEmailTemplate(IConfiguration configuration) : IEmailTemplate<ResetPasswordEmailModel>
{
    public string Subject => $"Password Reset for {configuration["Info:Name"]}";
    public string GenerateBody(ResetPasswordEmailModel model)
    {
        var resetUrl = $"{configuration["Info:FrontendUrl"]}/auth/reset-password?token={Uri.EscapeDataString(model.Token)}&email={model.Email}";
        return $"""
        <!DOCTYPE html>
        <html lang="en">
        <head>
            <meta charset="UTF-8" />
            <meta name="viewport" content="width=device-width, initial-scale=1.0" />
            <title>Password Reset</title>
        </head>
        <body style="background:#f5f5f5; padding:40px 0; font-family:Arial, sans-serif;">

            <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td align="center">
                        <table width="600" cellpadding="30" cellspacing="0" 
                               style="background:#ffffff; border-radius:12px; box-shadow:0 2px 8px rgba(0,0,0,0.1);">
                            <tr>
                                <td>

                                    <h2 style="color:#333; margin-top:0;">Hello {WebUtility.HtmlEncode(model.Fullname)},</h2>

                                    <p style="font-size:16px; color:#555;">
                                        We received a request to reset your password for your NYC360 account.
                                    </p>

                                    <p style="font-size:16px; color:#555;">
                                        To proceed, please click the button below:
                                    </p>

                                    <p style="text-align:center; margin:30px 0;">
                                        <a href="{resetUrl}" 
                                           style="
                                                display:inline-block;
                                                padding:14px 28px;
                                                background:#0066ff;
                                                color:white;
                                                text-decoration:none;
                                                border-radius:6px;
                                                font-size:16px;
                                                font-weight:bold;
                                                ">
                                            Reset Password
                                        </a>
                                    </p>

                                    <p style="font-size:15px; color:#777;">
                                        If the button above doesn't work, copy and paste the link below into your browser:
                                    </p>

                                    <p style="word-break:break-all; font-size:14px; color:#0066ff;">
                                        {resetUrl}
                                    </p>

                                    <hr style="margin:40px 0; border:0; border-top:1px solid #eee;" />

                                    <p style="font-size:14px; color:#999;">
                                        If you did not request this, you can safely ignore this email.
                                    </p>

                                </td>
                            </tr>
                        </table>

                        <p style="font-size:12px; color:#aaa; margin-top:20px;">
                            © {DateTime.UtcNow.Year} NYC360. All rights reserved.
                        </p>
                    </td>
                </tr>
            </table>

        </body>
        </html>
        """;
    }
}