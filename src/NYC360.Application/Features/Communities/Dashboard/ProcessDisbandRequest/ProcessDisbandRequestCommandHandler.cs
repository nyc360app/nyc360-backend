using MediatR;
using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Enums.Communities;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Communities.Dashboard.ProcessDisbandRequest;

public class ProcessDisbandRequestCommandHandler(
    ICommunityRepository communityRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<ProcessDisbandRequestCommand, StandardResponse<string>>
{
    public async Task<StandardResponse<string>> Handle(ProcessDisbandRequestCommand request, CancellationToken ct)
    {
        // 1. Verify disband request exists
        var disbandRequest = await communityRepository.GetDisbandRequestByIdAsync(request.RequestId, ct);
        if (disbandRequest == null)
        {
            return StandardResponse<string>.Failure(
                new ApiError("disbandRequest.notFound", "Disband request not found."));
        }

        // 2. Check if request is still pending
        if (disbandRequest.Status != DisbandRequestStatus.Pending)
        {
            return StandardResponse<string>.Failure(
                new ApiError("disbandRequest.alreadyProcessed", "This disband request has already been processed."));
        }

        // 3. Update request status
        disbandRequest.Status = request.Approved ? DisbandRequestStatus.Approved : DisbandRequestStatus.Rejected;
        disbandRequest.ProcessedAt = DateTime.UtcNow;
        disbandRequest.ProcessedByUserId = request.AdminUserId;
        disbandRequest.AdminNotes = request.AdminNotes;

        communityRepository.UpdateDisbandRequest(disbandRequest);

        // 4. If approved, delete the community
        if (request.Approved)
        {
            var community = await communityRepository.GetByIdAsync(disbandRequest.CommunityId, ct);
            if (community != null)
            {
                communityRepository.Remove(community);
            }
        }

        await unitOfWork.SaveChangesAsync(ct);

        return StandardResponse<string>.Success(
            request.Approved 
                ? "Disband request approved and community has been disbanded." 
                : "Disband request rejected.");
    }
}
