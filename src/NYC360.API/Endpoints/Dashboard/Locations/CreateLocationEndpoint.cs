using NYC360.Application.Features.Locations.Commands.CreateLocation;
using FastEndpoints;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Dashboard.Locations;

public class CreateLocationEndpoint(IMediator mediator)
    : Endpoint<CreateLocationCommand, StandardResponse<int>>
{
    public override void Configure()
    {
        Post("/locations-dashboard/create");
        Roles("SuperAdmin");
    }

    public override async Task HandleAsync(CreateLocationCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, cancellation: ct);
    }
}
