/*
using FastEndpoints;
using MediatR;
using NYC360.API.Extensions;
using NYC360.Application.Features.Events.Commands.Delete;
using NYC360.Domain.Wrappers;

using NYC360.API.Models.Events;

namespace NYC360.API.Endpoints.Public.Events;

public class DeleteEventEndpoint(IMediator mediator) : Endpoint<DeleteEventRequest, StandardResponse<bool>>
{
    public override void Configure()
    {
        Delete("/events/{id}");
        Permissions(Domain.Constants.Permissions.Events.Delete);
    }

    public override async Task HandleAsync(DeleteEventRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        
        if (userId == null)
        {
            await Send.OkAsync(StandardResponse<bool>.Failure(new ApiError("auth.unauthorized", "User not found")), ct);
            return;
        }

        var result = await mediator.Send(new DeleteEventCommand(req.Id, userId.Value), ct);
        await Send.OkAsync(result, ct);
    }
}
*/
