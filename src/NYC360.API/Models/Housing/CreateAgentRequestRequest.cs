using NYC360.Domain.Enums.Housing;

namespace NYC360.API.Models.Housing;

public record CreateAgentRequestRequest(
    int PostId,
    string? Name,
    string? Email,
    string? PhoneNumber,
    
    PreferredContactType PreferredContactType,
    
    DateOnly PreferredContactDate,
    TimeOnly PreferredContactTime,
    
    HouseholdType HouseholdType,
    
    DateOnly MoveInDate,
    DateOnly? MoveOutDate,
    
    DateOnly? ScheduleVirtualDate,
    TimeOnly? ScheduleVirtualTimeWindow,
    
    DateOnly? InPersonTourDate,
    TimeOnly? InPersonTourTimeWindow,
    string Message
);