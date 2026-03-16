using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Enums.Housing;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Housing.Commands.UpdateRequestStatus;

public class UpdateHousingRequestStatusCommandHandler(
    IHousingRequestRepository housingRequestRepository,
    IHouseInfoRepository houseInfoRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateHousingRequestStatusCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(UpdateHousingRequestStatusCommand request, CancellationToken ct)
    {
        // Prevent agents from setting status to Cancelled
        if (request.Status == HousingRequestStatus.Cancelled)
            return StandardResponse.Failure(new ApiError("housing.request.invalid_status", "Agents cannot set the status to Cancelled. Only users can cancel their own requests."));
        
        var housingRequest = await housingRequestRepository.GetByIdAsync(request.RequestId, ct);
        
        if (housingRequest is null)
            return StandardResponse.Failure(new ApiError("housing.request.notfound", "Housing request not found."));
        
        // Get the housing listing to verify the agent owns it
        var houseInfo = await houseInfoRepository.GetByIdAsync(housingRequest.HouseInfoId, ct);
        
        if (houseInfo is null)
            return StandardResponse.Failure(new ApiError("housing.post.notfound", "Housing listing not found."));
        
        // Verify that the agent owns the listing
        if (houseInfo.UserId != request.AgentUserId)
            return StandardResponse.Failure(new ApiError("housing.request.unauthorized", "You are not authorized to update this request. Only the listing owner can update request statuses."));
        
        // Don't allow updating if the request is already cancelled
        if (housingRequest.Status == HousingRequestStatus.Cancelled)
            return StandardResponse.Failure(new ApiError("housing.request.cancelled", "Cannot update a cancelled request."));
        
        // Update the status
        housingRequest.Status = request.Status;
        
        housingRequestRepository.Update(housingRequest);
        await unitOfWork.SaveChangesAsync(ct);

        return StandardResponse.Success();
    }
}
