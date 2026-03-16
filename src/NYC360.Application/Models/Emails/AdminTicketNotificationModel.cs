namespace NYC360.Application.Models.Emails;

public sealed record AdminTicketNotificationModel(
    string UserFullName, 
    string UserEmail,
    string Subject, 
    string Message
);