using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Entities.User;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Posts.Commands.SavePost;

public class SavePostCommandHandler(
    IUserSavedPostRepository userSavedPostRepository,
    IPostRepository postRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<SavePostCommand, StandardResponse<int>>
{
    public async Task<StandardResponse<int>> Handle(SavePostCommand request, CancellationToken cancellationToken)
    {
        var postExists = await postRepository.ExistsAsync(request.PostId, cancellationToken);
        if (!postExists) return StandardResponse<int>.Failure(new ApiError("post.notfound", "Post not found."));

        // Check if it already exists to determine if we Add or Remove
        var existingSave = await userSavedPostRepository.GetByUserAndPostIdAsync(request.UserId, request.PostId, cancellationToken);

        if (existingSave != null)
        {
            // UNSAVE: Remove the entry
            userSavedPostRepository.Remove(existingSave);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return StandardResponse<int>.Success(0); // Return 0 or a flag indicating unsaved
        }

        // SAVE: Create new entry
        var userSavedPost = new UserSavedPost
        {
            UserId = request.UserId,
            PostId = request.PostId,
            SavedAt = DateTime.UtcNow
        };

        await userSavedPostRepository.AddAsync(userSavedPost, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    
        return StandardResponse<int>.Success(userSavedPost.Id);
    }
}