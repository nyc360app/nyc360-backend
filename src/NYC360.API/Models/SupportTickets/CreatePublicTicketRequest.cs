namespace NYC360.API.Models.SupportTickets;

public record CreatePublicTicketRequest(string Email, string Name, string Subject, string Message);