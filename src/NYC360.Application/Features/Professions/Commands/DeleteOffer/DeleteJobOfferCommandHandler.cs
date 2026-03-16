using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Professions.Commands.DeleteOffer;

public class DeleteJobOfferCommandHandler(
    IProfessionsRepository professionsRepository, 
    IUnitOfWork unitOfWork) 
    : IRequestHandler<DeleteJobOfferCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(DeleteJobOfferCommand request, CancellationToken ct)
    {
        // 1. Verify Ownership & Existence
        var job = await professionsRepository.GetJobOfferForManagementAsync(request.OfferId, request.UserId, ct);
        
        if (job == null)
            return StandardResponse.Failure(new ApiError("job.not_found", "Job offer not found or access denied."));

        // 2. Delete
        professionsRepository.DeleteJobOffer(job);

        // 3. Persist
        await unitOfWork.SaveChangesAsync(ct);

        return StandardResponse.Success();
    }
}