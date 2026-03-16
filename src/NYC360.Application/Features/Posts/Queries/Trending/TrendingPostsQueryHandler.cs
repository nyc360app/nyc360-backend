using MediatR;
using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.Posts;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Posts.Queries.Trending;

public class TrendingPostsQueryHandler(IPostRepository postRepository)
    : IRequestHandler<TrendingPostsQuery, PagedResponse<PostDto>>
{
    public async Task<PagedResponse<PostDto>> Handle(TrendingPostsQuery request, CancellationToken ct)
    {
        // Call the new repository method with user interests
        var (items, total) = await postRepository.GetTrendingPaginatedAsync(
            request.Interests,
            request.Page,
            request.PageSize,
            request.UserId,
            ct);

        return PagedResponse<PostDto>.Create(
            items,
            request.Page,
            request.PageSize,
            total
        );
    }
}