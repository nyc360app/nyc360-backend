using MediatR;
using Microsoft.Extensions.Logging;
using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.Posts;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Posts.Commands.FeatureToggle;

public sealed class PostFeatureToggleCommandHandler(
    IPostRepository postRepository,
    IUnitOfWork unitOfWork,
    ILogger<PostFeatureToggleCommandHandler> logger)
    : IRequestHandler<PostFeatureToggleCommand, StandardResponse<PostFeatureStatusDto>>
{
    public async Task<StandardResponse<PostFeatureStatusDto>> Handle(PostFeatureToggleCommand request, CancellationToken ct)
    {
        var post = await postRepository.GetByIdAsync(request.PostId, ct);
        if (post is null)
            return StandardResponse<PostFeatureStatusDto>.Failure(new ApiError("post.notfound", "Post not found."));

        post.IsFeatured = request.IsFeatured;
        post.LastUpdated = DateTime.UtcNow;

        if (request.IsFeatured)
        {
            post.FeaturedAt = DateTime.UtcNow;
            post.FeaturedByUserId = request.ProcessedByUserId;
        }
        else
        {
            post.FeaturedAt = null;
            post.FeaturedByUserId = null;
        }

        postRepository.Update(post);
        await unitOfWork.SaveChangesAsync(ct);

        logger.LogInformation(
            "Post {PostId} feature status set to {IsFeatured} by admin {AdminUserId} at {ProcessedAt}",
            post.Id,
            post.IsFeatured,
            request.ProcessedByUserId,
            DateTime.UtcNow);

        return StandardResponse<PostFeatureStatusDto>.Success(new PostFeatureStatusDto(post.Id, post.IsFeatured));
    }
}
