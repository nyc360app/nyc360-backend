using NYC360.Domain.Enums.Housing;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Housing.Commands.CreateAgentRequest;

public record CreateAgentRequestCommand(
    int UserId,
    int HouseInfoId,
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
) : IRequest<StandardResponse<int>>;