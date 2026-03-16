using NYC360.Application.Features.Tags.Commands.Create;
using NYC360.Domain.Wrappers;
using NYC360.API.Models.Tags;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Dashboard.Tags;

public class CreateTagEndpoint(IMediator mediator) : Endpoint<CreateTagRequest, StandardResponse>
{
    public override void Configure()
    {
        Post("/tags-dashboard/create");
        Permissions(Domain.Constants.Permissions.Tags.Create);
    }

    public override async Task HandleAsync(CreateTagRequest req, CancellationToken ct)
    {
        var command = new CreateTagCommand(
            req.Name, 
            req.Type, 
            req.Division,
            req.ParentTagId);
        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, ct);
    }
}