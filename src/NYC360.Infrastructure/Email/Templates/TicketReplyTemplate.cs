using NYC360.Application.Contracts.Emails;
using Microsoft.Extensions.Configuration;
using NYC360.Application.Models.Emails;

namespace NYC360.Infrastructure.Email.Templates;

public sealed class TicketReplyTemplate(IConfiguration config) : IEmailTemplate<TicketReplyEmailModel>
{
    public string Subject => "Update regarding your NYC360 Support Ticket";

    public string GenerateBody(TicketReplyEmailModel model) => $@"
        <div style=""font-family: Arial, sans-serif; color: #333;"">
            <h2>Hi {model.UserFullName},</h2>
            <p>Our support team has responded to your ticket: <strong>#{model.TicketId} - {model.TicketSubject}</strong></p>
            <div style=""background: #f4f4f4; padding: 20px; border-left: 4px solid #0078d4; margin: 20px 0;"">
                <p style=""white-space: pre-wrap;"">{model.AdminReply}</p>
            </div>
            <p>If you have more questions, simply reply to this email.</p>
            <br>
            <p>Best regards,<br>NYC360 Support Team</p>
        </div>";
}