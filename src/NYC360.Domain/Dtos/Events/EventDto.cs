using NYC360.Domain.Dtos.Location;
using NYC360.Domain.Enums.Events;
using NYC360.Domain.Dtos.User;
using NYC360.Domain.Entities.Events;

namespace NYC360.Domain.Dtos.Events;

public record EventDto(
    int Id,
    List<string> Attachments,
    string Title,
    string Description,
    EventCategory Category,
    EventType Type,
    DateTime StartDateTime,
    DateTime? EndDateTime,
    EventStatus Status,
    EventVisibility Visibility,
    AddressDto? Address,
    List<TicketTierDto> Tiers,
    int OwnerId,
    List<UserMinimalInfoDto> Staff,
    int TotalTicketsSold
);

public static class EventDtoExtensions
{
    extension(EventDto)
    {
        public static EventDto Map(Event ev)
        {
            return new EventDto(
                Id: ev.Id,
                Attachments: ev.Attachments.Select(a => a.Url).ToList(),
                Title: ev.Title,
                Description: ev.Description,
                Category: ev.Category,
                Type: ev.Type,
                StartDateTime: ev.StartDateTime,
                EndDateTime: ev.EndDateTime,
                Status: ev.Status,
                Visibility: ev.Visibility ?? EventVisibility.Public,
                // Using the AddressDto mapper we fixed previously
                Address: AddressDtoExtensions.Map(ev.Address),
                Tiers: ev.Tiers.Select(TicketTierDto.Map).ToList(),
                OwnerId: ev.OwnerId ?? 0,
                // Map Staff to Minimal User Info (Régisseur Sheet 3: Accountability)
                Staff: ev.Staff
                    .Where(s => s.User != null)
                    .Select(s => UserMinimalInfoDto.Map(s.User))
                    .ToList(),
                // Computed Logic (Régisseur Sheet 7: Payouts/Liquidity)
                TotalTicketsSold: ev.Tiers.Sum(t => t.QuantitySold)
            );
        }
    }
}
