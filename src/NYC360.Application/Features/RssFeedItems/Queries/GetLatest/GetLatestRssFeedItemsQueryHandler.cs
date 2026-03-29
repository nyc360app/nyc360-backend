using MediatR;
using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.Rss;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.RssFeedItems.Queries.GetLatest;

public class GetLatestRssFeedItemsQueryHandler(IRssFeedItemRepository rssFeedItemRepository)
    : IRequestHandler<GetLatestRssFeedItemsQuery, StandardResponse<List<RssFeedItemDto>>>
{
    public async Task<StandardResponse<List<RssFeedItemDto>>> Handle(GetLatestRssFeedItemsQuery request, CancellationToken cancellationToken)
    {
        var limit = Math.Clamp(request.Limit, 1, 50);
        var items = await rssFeedItemRepository.GetLatestByCategoryAsync(request.Category, limit, cancellationToken);
        var dtos = items.Select(RssFeedItemDto.Map).ToList();

        return StandardResponse<List<RssFeedItemDto>>.Success(dtos);
    }
}
