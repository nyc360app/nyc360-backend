using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Entities.Locations;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Locations.Commands.CreateLocation;

public class CreateLocationCommandHandler(ILocationRepository locationRepository, IUnitOfWork unitOfWork) 
    : IRequestHandler<CreateLocationCommand, StandardResponse<int>>
{
    public async Task<StandardResponse<int>> Handle(CreateLocationCommand request, CancellationToken ct)
    {
        var location = new Location
        {
            Borough = request.Borough,
            Code = request.Code,
            NeighborhoodNet = request.NeighborhoodNet,
            Neighborhood = request.Neighborhood,
            ZipCode = request.ZipCode
        };

        await locationRepository.AddAsync(location, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return StandardResponse<int>.Success(location.Id);
    }
}
