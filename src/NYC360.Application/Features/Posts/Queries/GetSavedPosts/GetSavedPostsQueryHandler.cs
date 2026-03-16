using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.Posts;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Posts.Queries.GetSavedPosts;

public class GetSavedPostsQueryHandler(IPostRepository postRepository)
    : IRequestHandler<GetSavedPostsQuery, PagedResponse<PostDto>>
{
    public async Task<PagedResponse<PostDto>> Handle(GetSavedPostsQuery request, CancellationToken ct)
    {
        var (items, total) = await postRepository.GetSavedPostsPaginatedAsync(
            request.UserId, 
            request.Page, 
            request.PageSize, 
            request.Category,
            ct);

        return PagedResponse<PostDto>.Create(items, request.Page, request.PageSize, total);
    }
}