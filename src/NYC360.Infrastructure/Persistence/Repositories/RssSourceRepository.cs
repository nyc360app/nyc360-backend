using NYC360.Infrastructure.Persistence.DbContexts;
using NYC360.Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;
using NYC360.Domain.Entities;
using NYC360.Domain.Enums;

namespace NYC360.Infrastructure.Persistence.Repositories;

public class RssSourceRepository(ApplicationDbContext db) : IRssSourceRepository
{
    public async Task AddAsync(RssFeedSource source, CancellationToken ct)
    {
        await db.RssFeedSources.AddAsync(source, ct);
    }

    public async Task<RssFeedSource?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await db.RssFeedSources.FirstOrDefaultAsync(rss => rss.Id == id, ct);
    }

    public async Task<List<RssFeedSource>> GetAllAsync(Category? category, CancellationToken ct)
    {
        var query = db.RssFeedSources.AsNoTracking().AsQueryable();

        if (category.HasValue)
            query = query.Where(rss => rss.Category == category.Value);

        return await query.ToListAsync(ct);
    }

    public void Update(RssFeedSource source)
    {
        db.RssFeedSources.Update(source);
    }

    public void Update(List<RssFeedSource> sources)
    {
        db.RssFeedSources.UpdateRange(sources);
    }

    public void Remove(RssFeedSource source)
    {
        db.RssFeedSources.Remove(source);
    }

    public async Task<bool> ExistsAsync(string url, CancellationToken ct)
    {
        return await db.RssFeedSources.AnyAsync(rss => rss.RssUrl == url, ct);
    }

    public async Task SaveChangesAsync(CancellationToken ct) 
        => await db.SaveChangesAsync(ct);
}
