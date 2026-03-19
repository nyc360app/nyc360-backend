using Microsoft.EntityFrameworkCore;
using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Entities;
using NYC360.Domain.Enums;
using NYC360.Infrastructure.Persistence.DbContexts;

namespace NYC360.Infrastructure.Persistence.Repositories;

public class RssFeedConnectionRequestRepository(ApplicationDbContext dbContext)
    : GenericRepository<RssFeedConnectionRequest>(dbContext), IRssFeedConnectionRequestRepository
{
    public async Task<IReadOnlyList<RssFeedConnectionRequest>> GetAllWithDetailsAsync(CancellationToken ct)
    {
        return await dbContext.RssFeedConnectionRequests
            .Include(x => x.Requester)
                .ThenInclude(x => x!.User)
            .ToListAsync(ct);
    }

    public async Task<bool> HasPendingRequestAsync(string url, Category category, CancellationToken ct)
    {
        return await dbContext.RssFeedConnectionRequests.AnyAsync(
            x => x.Category == category
                 && x.Status == RssConnectionStatus.Pending
                 && x.Url == url,
            ct);
    }
    
    public async Task<(IReadOnlyList<RssFeedConnectionRequest> Items, int Count)> GetPagedRequestsAsync(int pageNumber, int pageSize, RssConnectionStatus? status, Category? category, CancellationToken ct)
    {
        var query = dbContext.RssFeedConnectionRequests
            .Include(x => x.Requester)
                .ThenInclude(x => x!.User)
            .AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(x => x.Status == status.Value);
        }

        if (category.HasValue)
        {
            query = query.Where(x => x.Category == category.Value);
        }

        query = query.OrderByDescending(x => x.CreatedAt);

        var count = await query.CountAsync(ct);
        
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
            
        return (items, count);
    }
}
