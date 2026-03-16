using NYC360.Domain.Dtos.Posts;
using NYC360.Domain.Enums;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Posts.Queries.Trending;

public record TrendingPostsQuery(
    int? UserId,
    List<Category> Interests,
    int Page,
    int PageSize
) : IRequest<PagedResponse<PostDto>>;