using NYC360.Domain.Entities.Events;

namespace NYC360.Application.Contracts.Infrastructure;

public interface IStripeService
{
    Task<string> CreateCheckoutSessionAsync(Event ev, EventTicketTier tier, int userId, CancellationToken ct);
}
