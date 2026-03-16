using NYC360.Domain.Dtos.Location;
using NYC360.Domain.Enums.Events;

namespace NYC360.API.Models.Events;

public record CreateEventRequest(
    string Title,
    string Description,
    
    EventCategory Category,
    EventType Type,
    EventRole UserRole,
    
    DateTime StartDateTime,
    DateTime? EndDateTime,
    
    AddressInputDto? Address,
    
    List<IFormFile>? Attachments
);
