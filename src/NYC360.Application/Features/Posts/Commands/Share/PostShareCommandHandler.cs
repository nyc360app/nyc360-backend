using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Entities.Posts;
using NYC360.Domain.Enums.Posts;
using NYC360.Domain.Dtos.Posts;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Posts.Commands.Share;

public class PostShareCommandHandler(
    IPostRepository postRepository,
    IPostInteractionRepository interactionRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<PostShareCommand, StandardResponse<PostDto>>
{
    public async Task<StandardResponse<PostDto>> Handle(PostShareCommand request, CancellationToken ct)
    {
        // 1. Fetch the original post
        var originalPost = await postRepository.GetByIdAsync(request.ParentPostId, ct);
        if (originalPost == null)
            return StandardResponse<PostDto>.Failure(new ApiError("posts.not_found", "Original post not found."));
        
        // 2. Create the Share Entity
        var sharePost = new Post
        {
            AuthorId = request.UserId,
            ParentPostId = originalPost.ParentPostId ?? originalPost.Id,
            SourceType = PostSource.User,
            PostType = PostType.Normal,
            
            // Commentary is the new content, Category is inherited
            Content = request.Commentary,
            Title = $"Shared: {(originalPost.ParentPostId != null ? originalPost.ParentPost!.Title : originalPost.Title)}", 
            Category = originalPost.Category,
            
            // User can override location, otherwise inherit it
            LocationId = originalPost.LocationId,
            
            IsApproved = true, // Shares usually inherit approval or are auto-approved
            CreatedAt = DateTime.UtcNow,
            LastUpdated = DateTime.UtcNow
        };

        // 3. Persist
        await postRepository.AddAsync(sharePost, ct);

        // 4. Update Stats on the original post
        await interactionRepository.IncrementShareCountAsync(request.ParentPostId, ct);

        await unitOfWork.SaveChangesAsync(ct);

        // 5. Return fully loaded DTO (including original post details)
        var result = await postRepository.GetPostByIdAsync(sharePost.Id, request.UserId, ct);
        return StandardResponse<PostDto>.Success(result!);
    }
}