using NYC360.Domain.Enums;

namespace NYC360.Domain.Dtos.Support;

public record SupportTicketDto(
    int Id, 
    string Subject, 
    string? CreatorName, 
    string? CreatorEmail, 
    SupportTicketStatus Status, 
    DateTime CreatedAt, 
    string? AssignedAdminName);