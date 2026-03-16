using NYC360.Domain.Dtos.Location;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Locations.Queries.SearchLocations;

public record SearchLocationsQuery(string SearchTerm, int Limit) 
    : IRequest<StandardResponse<List<LocationDto>>>;