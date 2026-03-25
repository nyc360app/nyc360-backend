using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Entities.Tags;
using NYC360.Domain.Wrappers;
using NYC360.Domain.Enums;
using NYC360.Domain.Enums.Tags;
using NYC360.Domain.Entities.User;
using MediatR;

namespace NYC360.Application.Features.Verifications.Commands.ResolveTagRequest;

public class ResolveTagVerificationHandler(
    IVerificationRepository verificationRepository,
    ITagRepository tagRepository,
    IUserRepository userRepository,
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
        
        // 2. Process Approval
        if (request.Approved)
        {
            vRequest.Status = VerificationStatus.Approved;
            vRequest.AdminComment = request.AdminComment;
            
            // The Key Logic: Grant the Tag to the User if it isn't already there.
            // This keeps approval idempotent for flows where a baseline tag may already exist.
            var exists = await verificationRepository.UserHasSpecificTagAsync(vRequest.UserId, vRequest.TargetTagId, ct);
            if (!exists)
            {
                var userTag = new UserTag
                {
                    UserId = vRequest.UserId,
                    TagId = vRequest.TargetTagId
                };

                await tagRepository.AddUserTagAsync(userTag, ct);
            }

            // Identity approvals must unlock frontend Gate 1 checks that rely on UserInfo.isVerified.
            var approvedTag = await tagRepository.GetByIdAsync(vRequest.TargetTagId);
            if (approvedTag?.Type == TagType.Identity)
            {
                var profile = await userRepository.GetByIdWithStatsAsync(vRequest.UserId, ct);
                if (profile == null)
                {
                    return StandardResponse.Failure(new ApiError("user.not_found", "User profile not found."));
                }

                if (profile.Stats == null)
                {
                    profile.Stats = new UserStats
                    {
                        UserId = profile.UserId,
                        IsVerified = true
                    };
                }
                else
                {
                    profile.Stats.IsVerified = true;
                }
            }
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
