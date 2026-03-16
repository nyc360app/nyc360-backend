namespace NYC360.Application.Models.Emails;

public sealed record SupportTicketModel(int Id, string FullName, string Email, string Subject, string Message);