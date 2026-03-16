namespace NYC360.Application.Models.Emails;

public sealed record TicketReplyEmailModel(
    string UserFullName, 
    string TicketSubject, 
    string AdminReply, 
    int TicketId);