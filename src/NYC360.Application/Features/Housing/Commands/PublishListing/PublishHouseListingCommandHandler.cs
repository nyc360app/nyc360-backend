using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Housing.Commands.PublishListing;

public class PublishHouseListingCommandHandler(
    IHouseInfoRepository houseInfoRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<PublishHouseListingCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(PublishHouseListingCommand request, CancellationToken ct)
    {
        var houseInfo = await houseInfoRepository.GetByIdAsync(request.HouseId, ct);
        
        if (houseInfo is null)
            return StandardResponse.Failure(new ApiError("housing.post.notfound", "Housing listing not found."));
        
        houseInfo.IsPublished = request.IsPublished;
        
        houseInfoRepository.Update(houseInfo);
        await unitOfWork.SaveChangesAsync(ct);

        return StandardResponse.Success();
    }
}
