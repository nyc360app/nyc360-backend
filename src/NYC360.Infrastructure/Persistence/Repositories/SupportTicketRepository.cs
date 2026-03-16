using NYC360.Infrastructure.Persistence.DbContexts;
using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Entities.Support;
using Microsoft.EntityFrameworkCore;
using NYC360.Domain.Dtos.Support;
using NYC360.Domain.Enums;

namespace NYC360.Infrastructure.Persistence.Repositories;

public sealed class SupportTicketRepository(ApplicationDbContext context) : ISupportTicketRepository
{
    public async Task AddAsync(SupportTicket ticket, CancellationToken ct)
    {
        await context.AddAsync(ticket, ct);
    }

    public async Task<SupportTicket?> GetByIdAsync(int ticketId, CancellationToken ct)
    {
        return await context.SupportTickets.FirstOrDefaultAsync(t => t.Id == ticketId, ct);
    }

    public async Task<(List<SupportTicketDto>, int)> GetPagedAsync(SupportTicketStatus? status, int page, int pageSize, CancellationToken ct)
    {
        var query = context.SupportTickets
            .AsNoTracking()
            .OrderByDescending(t => t.CreatedAt)
            .AsQueryable();

        // 1. Apply Filters
        if (status != null)
            query = query.Where(t => t.Status == status);
        
        var total = await query.CountAsync(ct);
        
        var items = await query.Select(t => new SupportTicketDto(
            t.Id,
            t.Subject,
            t.Name ?? t.Creator!.FirstName + " " + t.Creator!.LastName,
            t.Email ?? t.Creator!.Email,
            t.Status,
            t.CreatedAt,
            t.AssignedAdmin != null ? t.AssignedAdmin.FirstName : null
        )).ToListAsync(ct);

        return (items, total);
    }
}