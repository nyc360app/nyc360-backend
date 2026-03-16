using MediatR;
using Microsoft.AspNetCore.Http;
using NYC360.Domain.Dtos.Events;
using NYC360.Domain.Dtos.Location;
using NYC360.Domain.Enums.Events;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Events.Commands.Update;

public record UpdateEventCommand(
    int Id,
    int UserId,
    string Title,
    string Description,
    EventCategory Category,
    DateTime StartDateTime,
    DateTime EndDateTime,
    string VenueName,
    AddressInputDto? Address,
    EventVisibility Visibility,
    LocationDto? Location,
    List<TicketTierDto> Tiers,
    string? AccessPassword,
    IFormFile[]? Attachments
) : IRequest<StandardResponse<bool>>;
