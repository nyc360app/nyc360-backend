using NYC360.Domain.Dtos.Events;

namespace NYC360.API.Models.Events;

public record ManageTicketTiersRequest(
    int EventId,
    List<TicketTierDto> Tiers
);