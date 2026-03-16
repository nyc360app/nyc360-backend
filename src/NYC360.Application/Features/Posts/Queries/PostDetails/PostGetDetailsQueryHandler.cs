using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.Posts;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Posts.Queries.PostDetails;

public class PostGetDetailsQueryHandler(IPostRepository postRepository) 
    : IRequestHandler<PostGetDetailsQuery, StandardResponse<PostDetailsDto>>
{
    public async Task<StandardResponse<PostDetailsDto>> Handle(PostGetDetailsQuery request, CancellationToken ct)
    {
        var post = await postRepository.GetPostWithDetailsDtoByIdAsync(request.Id, request.UserId, ct);
        if (post is null)
            return StandardResponse<PostDetailsDto>.Failure(new ApiError("post.notfound", "Post not found."));
        
        return StandardResponse<PostDetailsDto>.Success(post);
    }
}