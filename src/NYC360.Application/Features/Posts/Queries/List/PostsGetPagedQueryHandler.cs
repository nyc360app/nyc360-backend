using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.Posts;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Posts.Queries.List;

public class PostsGetPagedQueryHandler(IPostRepository postRepository)
    : IRequestHandler<PostsGetPagedQuery, PagedResponse<PostDto>>
{
    public async Task<PagedResponse<PostDto>> Handle(PostsGetPagedQuery request, CancellationToken ct)
    {
        var (items, total) = await postRepository.GetAllPaginatedAsync(
            request.Page,
            request.PageSize,
            request.UserId,
            request.Category,
            request.Search,
            request.PostType,
            request.SourceType,
            null, // authorId (this is a general list query)
            ct);

        return PagedResponse<PostDto>.Create(
            items,
            request.Page,
            request.PageSize,
            total
        );
    }
}