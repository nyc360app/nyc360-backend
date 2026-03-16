using NYC360.Application.Features.Locations.Queries.SearchLocations;
using NYC360.API.Models.Locations;
using NYC360.Domain.Dtos.Location;
using NYC360.Domain.Wrappers;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.Locations;

public class SearchLocationEndpoint(IMediator mediator) : Endpoint<SearchLocationRequest, StandardResponse<List<LocationDto>>>
{
    public override void Configure()
    {
        Get("/locations/search");
        AllowAnonymous();
        Summary(s => {
            s.Description = "Search for NYC locations by neighborhood, borough, or code.";
            s.Params[nameof(SearchLocationRequest.Query)] = "The search term (min 2 characters).";
        });
    }

    public override async Task HandleAsync(SearchLocationRequest req, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(req.Query) || req.Query.Length < 2)
        {
            await Send.OkAsync(StandardResponse<List<LocationDto>>.Success(new()), ct);
            return;
        }

        var query = new SearchLocationsQuery(req.Query, req.Limit);
        var result = await mediator.Send(query, ct);
        
        await Send.OkAsync(result, ct);
    }
}