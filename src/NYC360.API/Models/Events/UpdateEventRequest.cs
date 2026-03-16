using NYC360.Domain.Dtos.Events;
using NYC360.Domain.Dtos.Location;
using NYC360.Domain.Enums.Events;

namespace NYC360.API.Models.Events;

public record UpdateEventRequest(
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
);
