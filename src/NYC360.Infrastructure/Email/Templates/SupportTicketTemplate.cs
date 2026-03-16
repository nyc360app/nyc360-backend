using NYC360.Application.Contracts.Emails;
using Microsoft.Extensions.Configuration;
using NYC360.Application.Models.Emails;

namespace NYC360.Infrastructure.Email.Templates;

public sealed class SupportTicketTemplate(IConfiguration config) : IEmailTemplate<SupportTicketModel>
{
    public string Subject => $"NYC360 - New Support Ticket Received";

    public string GenerateBody(SupportTicketModel model)
    {
        return $@"
        <div style=""font-family: Arial, sans-serif; line-height: 1.6; color: #333;"">
            <h2 style=""color: #0078d4;"">Hello, {model.FullName}</h2>
            <p>Thank you for reaching out to NYC360 Support. We have received your inquiry and a member of our team will get back to you shortly.</p>
            
            <div style=""background-color: #f4f4f4; padding: 15px; border-radius: 5px; margin: 20px 0;"">
                <h3 style=""margin-top: 0;"">Ticket #{model.Id}</h3>
                <p><strong>Subject:</strong> {model.Subject}</p>
                <p><strong>Message:</strong></p>
                <p style=""font-style: italic;"">""{model.Message}""</p>
            </div>

            <p>If you have additional information to add, please do not hesitate to reply to this email.</p>
            
            <hr style=""border: 0; border-top: 1px solid #eee; margin: 20px 0;"" />
            <p style=""font-size: 0.8em; color: #777;"">
                This is an automated confirmation from NYC360 Support. <br />
                Borough-wide assistance for all New Yorkers.
            </p>
        </div>";
    }
}