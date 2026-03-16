using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.Location;
using NYC360.Domain.Wrappers;
using NYC360.Domain.Entities.Locations;
using MediatR;

namespace NYC360.Application.Features.Locations.Queries.GetLocationsList;

public class GetLocationsListQueryHandler(ILocationRepository locationRepository) 
    : IRequestHandler<GetLocationsListQuery, PagedResponse<LocationDto>>
{
    public async Task<PagedResponse<LocationDto>> Handle(GetLocationsListQuery request, CancellationToken ct)
    {
        var (items, totalCount) = await locationRepository.GetPagedLocationsAsync(request.Page, request.PageSize, request.Search, ct);
        
        var dtos = items.Select(LocationDto.Map).ToList();

        return PagedResponse<LocationDto>.Create(
            data: dtos,
            page: request.Page,
            pageSize: request.PageSize,
            totalCount: totalCount
        );
    }
}
