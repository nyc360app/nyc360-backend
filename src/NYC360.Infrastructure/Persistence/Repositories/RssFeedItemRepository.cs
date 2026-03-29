using Microsoft.EntityFrameworkCore;
using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Entities;
using NYC360.Domain.Enums;
using NYC360.Infrastructure.Persistence.DbContexts;

namespace NYC360.Infrastructure.Persistence.Repositories;

public class RssFeedItemRepository(ApplicationDbContext dbContext) : IRssFeedItemRepository
{
    public async Task<bool> ExistsBySourceAndGuidAsync(int sourceId, string guid, CancellationToken ct)
    {
        return await dbContext.RssFeedItems.AnyAsync(
            x => x.SourceId == sourceId && x.Guid == guid,
            ct);
    }

    public async Task<bool> ExistsBySourceAndLinkHashAsync(int sourceId, string linkHash, CancellationToken ct)
    {
        return await dbContext.RssFeedItems.AnyAsync(
            x => x.SourceId == sourceId && x.LinkHash == linkHash,
            ct);
    }

    public async Task AddRangeAsync(List<RssFeedItem> items, CancellationToken ct)
    {
        await dbContext.RssFeedItems.AddRangeAsync(items, ct);
    }

    public async Task<List<RssFeedItem>> GetLatestByCategoryAsync(Category category, int limit, CancellationToken ct)
    {
        return await dbContext.RssFeedItems
            .AsNoTracking()
            .Where(x => x.IsActive && !x.IsDeleted && x.Category == category)
            .OrderByDescending(x => x.PublishedAt)
            .ThenByDescending(x => x.Id)
            .Take(limit)
            .ToListAsync(ct);
    }
}
