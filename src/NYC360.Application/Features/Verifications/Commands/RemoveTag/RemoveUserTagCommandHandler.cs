using MediatR;
using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Verifications.Commands.RemoveTag;

public class RemoveUserTagHandler(
    ITagRepository tagRepository,
    IVerificationRepository verificationRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<RemoveUserTagCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(RemoveUserTagCommand request, CancellationToken ct)
    {
        // 1. Verify the relationship exists
        var exists = await verificationRepository.UserHasSpecificTagAsync(request.UserId, request.TagId, ct);
        
        if (!exists)
        {
            return StandardResponse.Failure(new ApiError("tag.not_found", "User does not possess this tag."));
        }

        // 2. Remove the UserTag record
        // This is a direct deletion from the UserTag join table
        await tagRepository.RemoveUserTagAsync(request.UserId, request.TagId, ct);

        // 3. Persist changes
        await unitOfWork.SaveChangesAsync(ct);

        return StandardResponse.Success();
    }
}