using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.Posts;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Posts.Queries.GetPostsByTag;

public class GetPostsByTagHandler(IPostRepository postRepository)
    : IRequestHandler<GetPostsByTagQuery, PagedResponse<PostDto>>
{
    public async Task<PagedResponse<PostDto>> Handle(GetPostsByTagQuery request, CancellationToken ct)
    {
        var (posts, totalCount) = await postRepository.GetPostsByTagPaginatedAsync(
            request.TagName, 
            request.Page, 
            request.PageSize, 
            request.UserId, 
            ct);

        var result = PagedResponse<PostDto>.Create(posts, request.Page, request.PageSize, totalCount);
        
        return result;
    }
}