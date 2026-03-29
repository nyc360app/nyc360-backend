using MediatR;
using NYC360.Domain.Dtos.Rss;
using NYC360.Domain.Enums;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.RssFeedItems.Queries.GetLatest;

public record GetLatestRssFeedItemsQuery(
    Category Category,
    int Limit = 1) : IRequest<StandardResponse<List<RssFeedItemDto>>>;
