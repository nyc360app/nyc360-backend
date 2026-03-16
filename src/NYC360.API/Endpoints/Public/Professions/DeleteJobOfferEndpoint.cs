using NYC360.Application.Features.Professions.Commands.DeleteOffer;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.Professions;

public class DeleteJobOfferEndpoint(IMediator mediator) : EndpointWithoutRequest<StandardResponse>
{
    public override void Configure()
    {
        Delete("/professions/offers/{OfferId}/delete");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }
        
        
        var offerId = Route<int>("OfferId");
        var command = new DeleteJobOfferCommand(userId.Value, offerId);
        var result = await mediator.Send(command, ct);
        
        await Send.OkAsync(result, ct);
    }
}