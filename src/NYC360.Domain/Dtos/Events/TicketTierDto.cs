using NYC360.Domain.Entities.Events;

namespace NYC360.Domain.Dtos.Events;

public record TicketTierDto(
    int? Id,
    string Name,
    string Description,
    decimal? Price,
    int QuantityAvailable,
    int? MinPerOrder,
    int? MaxPerOrder,
    DateTime? SaleStart,
    DateTime? SaleEnd
);

public static class TicketTierDtoExtensions
{
    extension(TicketTierDto)
    {
        public static TicketTierDto Map(EventTicketTier tier) => new(
            tier.Id,
            tier.Name,
            tier.Description,
            tier.Price,
            tier.QuantityAvailable,
            tier.MinPerOrder,
            tier.MaxPerOrder,
            tier.SaleStart,
            tier.SaleEnd
        );
    }
}