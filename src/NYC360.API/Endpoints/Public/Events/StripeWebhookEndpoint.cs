using FastEndpoints;
using Microsoft.Extensions.Configuration;
using Stripe;

namespace NYC360.API.Endpoints.Public.Events;

public class StripeWebhookEndpoint(IConfiguration configuration) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Post("/webhooks/stripe");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        try
        {
            var stripeEvent = EventUtility.ConstructEvent(json,
                HttpContext.Request.Headers["Stripe-Signature"],
                configuration["Stripe:WebhookSecret"]);

            if (stripeEvent.Type == Stripe.EventTypes.CheckoutSessionCompleted)
            {
                var session = stripeEvent.Data.Object as Stripe.Checkout.Session;
                // ...
            }

            await Send.OkAsync(ct);
        }
        catch (StripeException)
        {
            await Send.ErrorsAsync(400, ct);
        }
    }
}
