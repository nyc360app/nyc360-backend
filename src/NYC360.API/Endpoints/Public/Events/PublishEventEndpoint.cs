using NYC360.Application.Features.Events.Commands.Publish;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.Events;

public class PublishEventEndpoint(IMediator mediator) : EndpointWithoutRequest<StandardResponse>
{
    public override void Configure()
    {
        Put("/events/{EventId}/management/publish");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }
        
        var eventId = Route<int>("EventId");
        
        var command = new PublishEventCommand(userId.Value, eventId);
        var result = await mediator.Send(command, ct);
        
        await Send.OkAsync(result, ct);
    }
}