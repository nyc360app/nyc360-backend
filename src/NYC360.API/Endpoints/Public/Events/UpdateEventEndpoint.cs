using FastEndpoints;
using MediatR;
using NYC360.API.Extensions;
using NYC360.Application.Features.Events.Commands.Update;
using NYC360.Domain.Wrappers;

using NYC360.API.Models.Events;

namespace NYC360.API.Endpoints.Public.Events;

public class UpdateEventEndpoint(IMediator mediator) : Endpoint<UpdateEventRequest, StandardResponse<bool>>
{
    public override void Configure()
    {
        Put("/events/{id}");
        Permissions(Domain.Constants.Permissions.Events.Update);
        AllowFileUploads();
    }

    public override async Task HandleAsync(UpdateEventRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.OkAsync(StandardResponse<bool>.Failure(new ApiError("auth.unauthorized", "User not found")), ct);
            return;
        }

        var id = Route<int>("id");
        
        var command = new UpdateEventCommand(
            id,
            userId.Value,
            req.Title,
            req.Description,
            req.Category,
            req.StartDateTime,
            req.EndDateTime,
            req.VenueName,
            req.Address,
            req.Visibility,
            req.Location,
            req.Tiers,
            req.AccessPassword,
            req.Attachments
        );
        
        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, ct);
    }
}
