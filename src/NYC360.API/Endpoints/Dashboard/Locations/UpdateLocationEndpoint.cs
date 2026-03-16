using NYC360.Application.Features.Locations.Commands.UpdateLocation;
using FastEndpoints;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Dashboard.Locations;

public class UpdateLocationEndpoint(IMediator mediator)
    : Endpoint<UpdateLocationCommand, StandardResponse>
{
    public override void Configure()
    {
        Put("/locations-dashboard/update/{id}");
        Roles("SuperAdmin");
    }

    public override async Task HandleAsync(UpdateLocationCommand command, CancellationToken ct)
    {
        // Ensure ID from route matches command ID or just use route ID
        var id = Route<int>("id");
        var actualCommand = command with { Id = id };
        
        var result = await mediator.Send(actualCommand, ct);
        await Send.OkAsync(result, cancellation: ct);
    }
}
