using NYC360.Infrastructure.Persistence.DbContexts;
using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Entities.Events;
using Microsoft.EntityFrameworkCore;

namespace NYC360.Infrastructure.Persistence.Repositories;

public sealed class EventRepository(ApplicationDbContext context) 
    : GenericRepository<Event>(context), IEventRepository
{
    public async Task<Event?> GetEventWithDetailsAsync(int id, CancellationToken ct)
    {
        return await context.Events
            .Include(e => e.Address)
            .Include(e => e.Tiers)
            .Include(e => e.Staff)
                .ThenInclude(s => s.User)
                    .ThenInclude(up => up.User)
            .Include(e => e.Owner)
            .Include(e => e.Attachments)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id, ct);
    }

    public async Task<(IReadOnlyList<Event> Items, int TotalCount)> GetPagedEventsAsync(
        string? searchTerm, 
        int category, 
        int status, 
        DateTime? fromDate, 
        DateTime? toDate, 
        int? locationId, 
        int pageNumber, 
        int pageSize, 
        CancellationToken ct)
    {
        var query = context.Events
            .Include(e => e.Address)
                .ThenInclude(a => a.Location)
            .Include(e => e.Address)
                .ThenInclude(a => a.ManagedByUser)
            
            .Include(e => e.Owner)
                .ThenInclude(o => o.User)
            
            .Include(e => e.Staff)
                .ThenInclude(s => s.User)
                    .ThenInclude(up => up.User)
            
            .Include(e => e.Tiers)
            .Include(e => e.Attachments)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchTerm))
            query = query.Where(e => e.Title.Contains(searchTerm) || e.Description.Contains(searchTerm));

        if (category > 0)
            query = query.Where(e => (int)e.Category == category);

        if (status > 0)
            query = query.Where(e => (int)e.Status == status);

        if (fromDate.HasValue)
            query = query.Where(e => e.StartDateTime >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(e => e.EndDateTime <= toDate.Value);

        if (locationId.HasValue)
            query = query.Where(e => e.Address!.LocationId == locationId.Value);

        var totalCount = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.StartDateTime)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, totalCount);
    }

    public async Task<Event?> GetEventWithTicketsAsync(int id, CancellationToken ct)
    {
        return await context.Events
            .Include(e => e.Tiers)
            .Include(e => e.Staff)
            .FirstOrDefaultAsync(e => e.Id == id, ct);
    }
}