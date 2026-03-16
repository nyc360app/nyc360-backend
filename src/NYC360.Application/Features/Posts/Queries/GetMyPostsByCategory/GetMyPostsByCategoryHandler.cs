using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.Posts;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Posts.Queries.GetMyPostsByCategory;

public class GetMyPostsByCategoryHandler(IPostRepository postRepository)
    : IRequestHandler<GetMyPostsByCategoryQuery, PagedResponse<PostDto>>
{
    public async Task<PagedResponse<PostDto>> Handle(GetMyPostsByCategoryQuery request, CancellationToken ct)
    {
        var (posts, totalCount) = await postRepository.GetAllPaginatedAsync(
            request.Page, 
            request.PageSize, 
            request.UserId, // For interactions
            request.Category,
            null, // search
            null, // postType
            null, // sourceType
            request.UserId, // authorId (filtering for ONLY current user's posts)
            ct);

        var result = PagedResponse<PostDto>.Create(posts, request.Page, request.PageSize, totalCount);
        
        return result;
    }
}
