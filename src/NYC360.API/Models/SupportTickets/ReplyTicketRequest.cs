namespace NYC360.API.Models.SupportTickets;

public record ReplyTicketRequest(int TicketId, string ReplyMessage);