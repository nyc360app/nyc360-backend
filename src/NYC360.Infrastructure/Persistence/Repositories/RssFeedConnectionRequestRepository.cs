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
    
    public async Task<(IReadOnlyList<RssFeedConnectionRequest> Items, int Count)> GetPagedRequestsAsync(int pageNumber, int pageSize, RssConnectionStatus? status, CancellationToken ct)
    {
        var query = dbContext.RssFeedConnectionRequests
            .Include(x => x.Requester)
                .ThenInclude(x => x!.User)
            .AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(x => x.Status == status.Value);
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
