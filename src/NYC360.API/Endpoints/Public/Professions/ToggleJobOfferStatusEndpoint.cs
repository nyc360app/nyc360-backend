using NYC360.Application.Features.Professions.Commands.ToggleOfferStatus;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.Professions;

public class ToggleJobOfferStatusEndpoint(IMediator mediator) : EndpointWithoutRequest<StandardResponse>
{
    public override void Configure()
    {
        Patch("/professions/offers/{OfferId}/toggle");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = User.GetId();

        if (userId == null)
        {
            await Send.ForbiddenAsync(ct);
            return;
        }
        var offerId = Route<int>("OfferId");
        var command = new ToggleOfferStatusCommand(userId.Value, offerId);
        var result = await mediator.Send(command, ct);
        
        await Send.OkAsync(result, ct);
    }
}