using NYC360.Application.Features.Locations.Queries.GetLocationById;
using FastEndpoints;
using MediatR;
using NYC360.Domain.Dtos.Location;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Dashboard.Locations;

public class GetLocationByIdEndpoint(IMediator mediator)
    : EndpointWithoutRequest<StandardResponse<LocationDto>>
{
    public override void Configure()
    {
        Get("/locations-dashboard/{id}");
        Roles("SuperAdmin");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<int>("id");
        var query = new GetLocationByIdQuery(id);
        var result = await mediator.Send(query, ct);

        await Send.OkAsync(result, cancellation: ct);
    }
}
