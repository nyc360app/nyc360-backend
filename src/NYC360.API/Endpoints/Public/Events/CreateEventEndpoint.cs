using NYC360.Application.Features.Events.Commands.Create;
using NYC360.API.Models.Events;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.Events;

public class CreateEventEndpoint(IMediator mediator) : Endpoint<CreateEventRequest, StandardResponse<int>>
{
    public override void Configure()
    {
        Post("/events/create");
        AllowFileUploads();
    }

    public override async Task HandleAsync(CreateEventRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }
        
        var command = new CreateEventCommand(
            userId.Value,
            req.Title,
            req.Description,
            req.Category,
            req.Type,
            req.UserRole,
            req.StartDateTime,
            req.EndDateTime,
            req.Address,
            req.Attachments
        );
        
        var result = await mediator.Send(command, ct);

        await Send.OkAsync(result, ct);
    }
}