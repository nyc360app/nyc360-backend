using NYC360.Application.Contracts.Persistence;
using Microsoft.AspNetCore.Identity;
using NYC360.Domain.Entities.Posts;
using NYC360.Domain.Entities.User;
using NYC360.Domain.Enums;
using MediatR;
using NYC360.Domain.Enums.Posts;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Posts.Commands.Interaction;

public sealed class PostInteractionCommandHandler(
    IPostRepository postRepository,
    IPostInteractionRepository interactionRepository,
    IUnitOfWork unitOfWork,
    UserManager<ApplicationUser> userManager) 
    : IRequestHandler<PostInteractionCommand, StandardResponse<InteractionType?>>
{
    public async Task<StandardResponse<InteractionType?>> Handle(PostInteractionCommand request, CancellationToken ct)
    {
        var user = await userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
            return StandardResponse<InteractionType?>.Failure(new ApiError("auth.notfound", "User not found."));

        var post = await postRepository.GetByIdWithStatsAsync(request.PostId, ct);
        if (post == null)
            return StandardResponse<InteractionType?>.Failure(new ApiError("post.notfound", "Post not found."));

        var existing = await interactionRepository.GetInteractionAsync(request.PostId, request.UserId, ct);

        InteractionType? previous = existing?.Type;     // IMPORTANT
        InteractionType? next = request.Interaction;
        InteractionType? finalState;

        // --------------------------
        // 1) SWITCH
        // --------------------------
        if (existing != null && previous != next)
        {
            existing.Type = next.Value;
            finalState = next;

            // update stats: previous → next
            if (previous == InteractionType.Like)
                post.Stats.Likes--;
            else if (previous == InteractionType.Dislike)
                post.Stats.Dislikes--;

            if (next == InteractionType.Like)
                post.Stats.Likes++;
            else if (next == InteractionType.Dislike)
                post.Stats.Dislikes++;

        }
        // --------------------------
        // 2) REMOVE
        // --------------------------
        else if (existing != null && previous == next)
        {
            interactionRepository.RemoveInteraction(existing);
            finalState = null;

            if (previous == InteractionType.Like)
                post.Stats.Likes--;
            else if (previous == InteractionType.Dislike)
                post.Stats.Dislikes--;
        }
        // --------------------------
        // 3) NEW
        // --------------------------
        else
        {
            var newInteraction = new PostInteraction
            {
                UserId = request.UserId,
                PostId = request.PostId,
                Type = next!.Value,
                CreatedAt = DateTime.UtcNow
            };

            await interactionRepository.AddInteractionAsync(newInteraction, ct);
            finalState = next;

            if (next == InteractionType.Like)
                post.Stats.Likes++;
            else if (next == InteractionType.Dislike)
                post.Stats.Dislikes++;
        }

        await unitOfWork.SaveChangesAsync(ct);

        return StandardResponse<InteractionType?>.Success(finalState);
    }
}