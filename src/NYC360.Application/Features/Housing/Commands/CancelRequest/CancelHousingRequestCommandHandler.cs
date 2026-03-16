using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Enums.Housing;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Housing.Commands.CancelRequest;

public class CancelHousingRequestCommandHandler(
    IHousingRequestRepository housingRequestRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CancelHousingRequestCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(CancelHousingRequestCommand request, CancellationToken ct)
    {
        var housingRequest = await housingRequestRepository.GetByIdAsync(request.RequestId, ct);
        
        if (housingRequest is null)
            return StandardResponse.Failure(new ApiError("housing.request.notfound", "Housing request not found."));
        
        // Verify that the user owns this request
        if (housingRequest.UserId != request.UserId)
            return StandardResponse.Failure(new ApiError("housing.request.unauthorized", "You are not authorized to cancel this request."));
        
        // Check if the request is already cancelled or closed
        if (housingRequest.Status == HousingRequestStatus.Cancelled)
            return StandardResponse.Failure(new ApiError("housing.request.already_cancelled", "This request has already been cancelled."));
        
        if (housingRequest.Status == HousingRequestStatus.Closed)
            return StandardResponse.Failure(new ApiError("housing.request.closed", "Cannot cancel a closed request."));
        
        // Update the status to Cancelled
        housingRequest.Status = HousingRequestStatus.Cancelled;
        
        housingRequestRepository.Update(housingRequest);
        await unitOfWork.SaveChangesAsync(ct);

        return StandardResponse.Success();
    }
}
