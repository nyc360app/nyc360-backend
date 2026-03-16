using NYC360.Application.Features.Locations.Queries.GetLocationsList;
using NYC360.API.Models.Locations;
using FastEndpoints;
using MediatR;
using NYC360.Domain.Dtos.Location;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Dashboard.Locations;

public class GetLocationsListEndpoint(IMediator mediator)
    : Endpoint<GetLocationsListRequest, PagedResponse<LocationDto>>
{
    public override void Configure()
    {
        Get("/locations-dashboard/list");
        Roles("SuperAdmin");
    }

    public override async Task HandleAsync(GetLocationsListRequest req, CancellationToken ct)
    {
        var query = new GetLocationsListQuery(req.Page, req.PageSize, req.Search);
        var result = await mediator.Send(query, ct);
        await Send.OkAsync(result, cancellation: ct);
    }
}
