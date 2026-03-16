using NYC360.Domain.Enums.Posts;
using NYC360.Domain.Dtos.Posts;
using NYC360.Domain.Wrappers;
using NYC360.Domain.Enums;
using MediatR;

namespace NYC360.Application.Features.Posts.Queries.GetUniversalFeed;

public record GetUniversalFeedQuery(
    int UserId,
    Category? Category,
    int? LocationId,
    string? Search,
    PostType? Type,
    int Page,
    int PageSize
) : IRequest<PagedResponse<PostDto>>;