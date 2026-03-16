using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Professions.Commands.ToggleOfferStatus;

public class ToggleOfferStatusHandler(
    IProfessionsRepository professionsRepository, 
    IUnitOfWork unitOfWork) 
    : IRequestHandler<ToggleOfferStatusCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(ToggleOfferStatusCommand request, CancellationToken ct)
    {
        // 1. Verify Ownership
        var job = await professionsRepository.GetJobOfferForManagementAsync(request.OfferId, request.UserId, ct);
        
        if (job == null)
            return StandardResponse.Failure(new ApiError("job.not_found", "Job offer not found or access denied."));

        // 2. Flip the Status
        job.IsActive = !job.IsActive;

        // 3. Persist
        await unitOfWork.SaveChangesAsync(ct);

        return StandardResponse.Success();
    }
}