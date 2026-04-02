using MediatR;
using NYC360.Domain.Dtos.News;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.News.Queries.GetFeaturedFeed;

public record GetFeaturedNewsFeedQuery(
    int? UserId,
    int PageSize,
    int Page,
    string? Cursor
) : IRequest<StandardResponse<NewsFeaturedFeedDto>>;
