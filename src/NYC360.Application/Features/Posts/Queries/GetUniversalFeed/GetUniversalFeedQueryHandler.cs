using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.Posts;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Posts.Queries.GetUniversalFeed;

public class GetUniversalFeedQueryHandler(IPostRepository postRepo) 
    : IRequestHandler<GetUniversalFeedQuery, PagedResponse<PostDto>>
{
    public async Task<PagedResponse<PostDto>> Handle(GetUniversalFeedQuery request, CancellationToken ct)
    {
        var (posts, total) = await postRepo.GetUniversalPostsAsync(
            request.UserId,
            request.Category,
            request.LocationId,
            request.Search,
            request.Type,
            request.Page,
            request.PageSize,
            ct);

        return PagedResponse<PostDto>.Create(posts, request.Page, request.PageSize, total);
    }
}