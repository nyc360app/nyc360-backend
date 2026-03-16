using NYC360.Domain.Dtos.Location;
using NYC360.Domain.Enums.Events;
using Microsoft.AspNetCore.Http;
using MediatR;
using NYC360.Domain.Wrappers;
using NYC360.Domain.Dtos.Events;

namespace NYC360.Application.Features.Events.Commands.Create;

public record CreateEventCommand(
    int UserId,
    string Title,
    string Description,
    EventCategory Category,
    EventType Type,
    EventRole UserRole,
    DateTime StartDateTime,
    DateTime? EndDateTime,
    AddressInputDto? Address,
    List<IFormFile>? Attachments
) : IRequest<StandardResponse<int>>;
