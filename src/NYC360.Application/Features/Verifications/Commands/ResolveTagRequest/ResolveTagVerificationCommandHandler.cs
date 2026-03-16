using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Entities.Tags;
using NYC360.Domain.Wrappers;
using NYC360.Domain.Enums;
using MediatR;

namespace NYC360.Application.Features.Verifications.Commands.ResolveTagRequest;

public class ResolveTagVerificationHandler(
    IVerificationRepository verificationRepository,
    ITagRepository tagRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<ResolveTagVerificationCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(ResolveTagVerificationCommand request, CancellationToken ct)
    {
        // 1. Fetch the request with the user and target tag info
        var vRequest = await verificationRepository.GetByIdAsync(request.RequestId, ct);
        if (vRequest == null) 
            return StandardResponse.Failure(new ApiError("verify.not_found", "Request not found."));

        if (vRequest.Status != VerificationStatus.Pending)
            return StandardResponse.Failure(new ApiError("verify.already_processed", "This request has already been handled."));
        
        var exists = await verificationRepository.UserHasSpecificTagAsync(vRequest.UserId, vRequest.TargetTagId, ct);
        if (exists) 
            return StandardResponse.Failure(new ApiError("verify.exists", "User already have this tag."));
        
        // 2. Process Approval
        if (request.Approved)
        {
            vRequest.Status = VerificationStatus.Approved;
            vRequest.AdminComment = request.AdminComment;
            
            // The Key Logic: Grant the Tag to the User
            var userTag = new UserTag 
            { 
                UserId = vRequest.UserId, 
                TagId = vRequest.TargetTagId 
            };

            // Using the UserTag entity we created in the registration logic
            await tagRepository.AddUserTagAsync(userTag, ct);
        }
        else
        {
            vRequest.Status = VerificationStatus.Rejected;
            vRequest.AdminComment = request.AdminComment;
        }

        vRequest.ProcessedAt = DateTime.UtcNow;
        
        // 3. Persist all changes in one transaction
        await unitOfWork.SaveChangesAsync(ct);

        return StandardResponse.Success();
    }
}