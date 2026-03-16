using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.Location;
using NYC360.Domain.Wrappers;
using NYC360.Domain.Entities.Locations;
using MediatR;

namespace NYC360.Application.Features.Locations.Queries.GetLocationById;

public class GetLocationByIdQueryHandler(ILocationRepository locationRepository) 
    : IRequestHandler<GetLocationByIdQuery, StandardResponse<LocationDto>>
{
    public async Task<StandardResponse<LocationDto>> Handle(GetLocationByIdQuery request, CancellationToken ct)
    {
        var location = await locationRepository.GetByIdAsync(request.Id, ct);
        
        if (location == null)
            return StandardResponse<LocationDto>.Failure(new ApiError("locations.not_found", "Location not found."));

        return StandardResponse<LocationDto>.Success(LocationDto.Map(location));
    }
}
