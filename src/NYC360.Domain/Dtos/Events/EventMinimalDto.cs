using NYC360.Domain.Entities.Events;
using NYC360.Domain.Dtos.Location;
using NYC360.Domain.Enums.Events;
using NYC360.Domain.Dtos.User;

namespace NYC360.Domain.Dtos.Events;

public record EventMinimalDto(
    int Id,
    string? ImageUrl,
    string Title,
    string Description,
    EventCategory Category,
    EventType Type,
    DateTime StartDateTime,
    DateTime? EndDateTime,
    EventStatus Status,
    EventVisibility? Visibility,
    decimal? PriceStart,
    decimal? PriceEnd,
    DateTime? SaleStart,
    DateTime? SaleEnd,
    AddressDto? Venue,
    UserMinimalInfoDto? PrimaryOrganizer
);

public static class EventMinimalDtoExtensions
{
    extension(EventMinimalDto)
    {
        public static EventMinimalDto Map(Event evt)
        {
            var activeTiers = evt.Tiers.Where(t => t.IsActive).ToList();
            var priceStart = activeTiers.Any() ? activeTiers.Min(t => t.Price) : null;
            var priceEnd = activeTiers.Any() ? activeTiers.Max(t => t.Price) : null;
            var saleStart = activeTiers.Any() ? activeTiers.Min(t => t.SaleStart) : null;
            var saleEnd = activeTiers.Any() ? activeTiers.Max(t => t.SaleEnd) : null;

            // 3. Identify Primary Organizer (Sheet 3: Role Definition)
            // We look for the main Organizer role in the staff collection
            var primaryStaff = evt.Staff
                .FirstOrDefault(s => s.Role == EventRole.Organizer);

            return new EventMinimalDto(
                evt.Id,
                evt.Attachments.FirstOrDefault()?.Url,
                evt.Title,
                evt.Description,
                evt.Category,
                evt.Type,
                evt.StartDateTime,
                evt.EndDateTime,
                evt.Status,
                evt.Visibility,
                priceStart,
                priceEnd,
                saleStart,
                saleEnd,
                AddressDtoExtensions.Map(evt.Address), 
                UserMinimalInfoDto.Map(evt.Owner!)
            );
        } 
    }
}