using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.Location;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Locations.Queries.SearchLocations;

public class SearchLocationsHandler(ILocationRepository locationRepository) 
    : IRequestHandler<SearchLocationsQuery, StandardResponse<List<LocationDto>>>
{
    public async Task<StandardResponse<List<LocationDto>>> Handle(SearchLocationsQuery request, CancellationToken ct)
    {
        var locations = await locationRepository.SearchLocationsAsync(request.SearchTerm, request.Limit, ct);
        var dtos = locations.Select(LocationDto.Map).ToList();

        return StandardResponse<List<LocationDto>>.Success(dtos);
    }
}