using NYC360.Application.Features.Locations.Commands.DeleteLocation;
using FastEndpoints;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Dashboard.Locations;

public class DeleteLocationEndpoint(IMediator mediator)
    : EndpointWithoutRequest<StandardResponse>
{
    public override void Configure()
    {
        Delete("/locations-dashboard/delete/{id}");
        Roles("Admin", "SuperAdmin");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<int>("id");
        var command = new DeleteLocationCommand(id);
        var result = await mediator.Send(command, ct);

        await Send.OkAsync(result, cancellation: ct);
    }
}
