using NYC360.Application.Features.Tags.Commands.Update;
using NYC360.API.Models.Tags;
using NYC360.Domain.Wrappers;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Dashboard.Tags;

public class UpdateTagEndpoint(IMediator mediator) : Endpoint<UpdateTagRequest, StandardResponse>
{
    public override void Configure()
    {
        Put("/tags-dashboard/update/{id}");
        Permissions(Domain.Constants.Permissions.Tags.Edit);
    }

    public override async Task HandleAsync(UpdateTagRequest req, CancellationToken ct)
    {
        var id = Route<int>("id");
        var command = new UpdateTagCommand(
            id,
            req.Name,
            req.Type,
            req.Division,
            req.ParentTagId);
            
        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, ct);
    }
}