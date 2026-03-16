using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Entities.Posts;
using NYC360.Domain.Enums;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Posts.Commands.SubmitFlag;

public class SubmitPostFlagCommandHandler(
    IPostRepository postRepository,
    IPostFlagRepository postFlagRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<SubmitPostFlagCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(SubmitPostFlagCommand request, CancellationToken ct)
    {
        // Validation: Ensure the post exists before allowing a flag
        var postExists = await postRepository.ExistsAsync(request.PostId, ct);
        if (!postExists)
        {
            var error = new ApiError("post.not_found", $"Post with ID {request.PostId} not found.");
            return StandardResponse.Failure(error);
        }

        // Create the Flag entity
        var newFlag = new PostFlag
        {
            PostId = request.PostId,
            UserId = request.UserId,
            Reason = request.Reason,
            Details = request.Details,
            Status = FlagStatus.Pending // Always starts as Pending
        };

        // Save the Flag
        await postFlagRepository.AddAsync(newFlag, ct);
        await unitOfWork.SaveChangesAsync(ct);
        
        return StandardResponse.Success();
    }
}