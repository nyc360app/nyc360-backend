using Microsoft.Extensions.Configuration;
using NYC360.Application.Contracts.Infrastructure;
using NYC360.Domain.Entities.Events;
using Stripe;
using Stripe.Checkout;

namespace NYC360.Infrastructure.Stripe;

public class StripeService : IStripeService
{
    private readonly IConfiguration _configuration;

    public StripeService(IConfiguration configuration)
    {
        _configuration = configuration;
        StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
    }

    public async Task<string> CreateCheckoutSessionAsync(NYC360.Domain.Entities.Events.Event ev, EventTicketTier tier, int userId, CancellationToken ct)
    {
        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = new List<string> { "card" },
            LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(tier.Price * 100), // Stripe expects cents
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = $"{ev.Title} - {tier.Name}",
                            Description = ev.Description,
                        },
                    },
                    Quantity = 1,
                },
            },
            Mode = "payment",
            SuccessUrl = _configuration["Stripe:SuccessUrl"] + "?session_id={CHECKOUT_SESSION_ID}",
            CancelUrl = _configuration["Stripe:CancelUrl"],
            Metadata = new Dictionary<string, string>
            {
                { "EventId", ev.Id.ToString() },
                { "TierId", tier.Id.ToString() },
                { "UserId", userId.ToString() }
            }
        };

        var service = new SessionService();
        var session = await service.CreateAsync(options, cancellationToken: ct);

        return session.Url;
    }
}
