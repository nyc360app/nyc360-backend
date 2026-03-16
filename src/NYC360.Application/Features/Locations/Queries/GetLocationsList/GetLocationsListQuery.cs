using MediatR;
using NYC360.Domain.Dtos.Location;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Locations.Queries.GetLocationsList;

public record GetLocationsListQuery(int Page, int PageSize, string? Search = null) 
    : PagedRequest(Page, PageSize), IRequest<PagedResponse<LocationDto>>;
