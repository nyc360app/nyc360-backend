using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Wrappers;
using NYC360.Domain.Entities.Locations;
using MediatR;

namespace NYC360.Application.Features.Locations.Commands.UpdateLocation;

public class UpdateLocationCommandHandler(ILocationRepository locationRepository, IUnitOfWork unitOfWork) 
    : IRequestHandler<UpdateLocationCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(UpdateLocationCommand request, CancellationToken ct)
    {
        var location = await locationRepository.GetByIdAsync(request.Id, ct);
        
        if (location == null)
            return StandardResponse.Failure(new ApiError("locations.not_found", "Location not found"));

        location.Borough = request.Borough;
        location.Code = request.Code;
        location.NeighborhoodNet = request.NeighborhoodNet;
        location.Neighborhood = request.Neighborhood;
        location.ZipCode = request.ZipCode;

        locationRepository.Update(location);
        await unitOfWork.SaveChangesAsync(ct);

        return StandardResponse.Success();
    }
}
