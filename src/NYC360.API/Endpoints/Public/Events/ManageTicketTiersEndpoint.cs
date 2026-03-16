using NYC360.Application.Features.Events.Commands.ManageTickets;
using NYC360.API.Models.Events;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.Events;

public class ManageTicketTiersEndpoint(IMediator mediator) : Endpoint<ManageTicketTiersRequest, StandardResponse>
{
    public override void Configure()
    {
        Put("/events/{EventId}/management/tickets/edit");
    }
    
    public override async Task HandleAsync(ManageTicketTiersRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }
        
        var command = new ManageTicketTiersCommand(
            userId.Value,
            req.EventId,
            req.Tiers
        );
        
        var result = await mediator.Send(command, ct);

        await Send.OkAsync(result, ct);
    }
}