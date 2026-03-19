using NYC360.Domain.Enums;
using MediatR;
using NYC360.Domain.Dtos.Posts;
using NYC360.Domain.Enums.Posts;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Posts.Queries.List;

public record PostsGetPagedQuery(
    int? UserId,
    int Page,
    int PageSize,
    Category? Category,
    string? Search,
    PostType? PostType,
    PostSource? SourceType,
    bool IncludeUnapproved = false
) : IRequest<PagedResponse<PostDto>>;
