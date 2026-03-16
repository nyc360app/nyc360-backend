using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Wrappers;
using NYC360.Domain.Entities.Locations;
using MediatR;

namespace NYC360.Application.Features.Locations.Commands.DeleteLocation;

public class DeleteLocationCommandHandler(ILocationRepository locationRepository, IUnitOfWork unitOfWork) 
    : IRequestHandler<DeleteLocationCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(DeleteLocationCommand request, CancellationToken ct)
    {
        var location = await locationRepository.GetByIdAsync(request.Id, ct);
        
        if (location == null)
            return StandardResponse.Failure(new ApiError("locations.notfound", "Location not found"));

        locationRepository.Remove(location);
        await unitOfWork.SaveChangesAsync(ct);

        return StandardResponse.Success();
    }
}
