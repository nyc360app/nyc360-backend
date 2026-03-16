using FastEndpoints;
using MediatR;
using NYC360.API.Extensions;
using NYC360.Application.Features.Events.Commands.Purchase;
using NYC360.Domain.Wrappers;

using NYC360.API.Models.Events;

namespace NYC360.API.Endpoints.Public.Events;

public class PurchaseTicketEndpoint(IMediator mediator) : Endpoint<PurchaseTicketRequest, StandardResponse<string>>
{
    public override void Configure()
    {
        Post("/events/{eventId}/purchase/{tierId}");
        Permissions(Domain.Constants.Permissions.Events.Purchase);
    }

    public override async Task HandleAsync(PurchaseTicketRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.OkAsync(StandardResponse<string>.Failure(new ApiError("auth.unauthorized", "User not found")), ct);
            return;
        }

        var result = await mediator.Send(new PurchaseTicketCommand(req.EventId, req.TierId, userId.Value), ct);
        await Send.OkAsync(result, ct);
    }
}
