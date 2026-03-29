using NYC360.Domain.Entities;
using NYC360.Domain.Enums;

namespace NYC360.Application.Contracts.Persistence;

public interface IRssFeedItemRepository
{
    Task<bool> ExistsBySourceAndGuidAsync(int sourceId, string guid, CancellationToken ct);
    Task<bool> ExistsBySourceAndLinkHashAsync(int sourceId, string linkHash, CancellationToken ct);
    Task AddRangeAsync(List<RssFeedItem> items, CancellationToken ct);
    Task<List<RssFeedItem>> GetLatestByCategoryAsync(Category category, int limit, CancellationToken ct);
}
