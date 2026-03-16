using MediatR;
using NYC360.Application.Contracts.Infrastructure;
using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Events.Commands.Purchase;

public class PurchaseTicketCommandHandler(
    IEventRepository eventRepository,
    IStripeService stripeService)
    : IRequestHandler<PurchaseTicketCommand, StandardResponse<string>>
{
    public async Task<StandardResponse<string>> Handle(PurchaseTicketCommand request, CancellationToken ct)
    {
        var ev = await eventRepository.GetEventWithDetailsAsync(request.EventId, ct);

        if (ev == null)
            return StandardResponse<string>.Failure(new ApiError("event.notFound", "Event not found"));

        var tier = ev.Tiers.FirstOrDefault(t => t.Id == request.TierId);
        if (tier == null)
            return StandardResponse<string>.Failure(new ApiError("tier.notFound", "Ticket tier not found"));

        if (tier.QuantitySold >= tier.QuantityAvailable)
            return StandardResponse<string>.Failure(new ApiError("tier.soldOut", "This ticket tier is sold out"));

        var checkoutUrl = await stripeService.CreateCheckoutSessionAsync(ev, tier, request.UserId, ct);
        
        return StandardResponse<string>.Success(checkoutUrl);
    }
}
