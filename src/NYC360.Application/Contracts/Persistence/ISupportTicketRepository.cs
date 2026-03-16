using NYC360.Domain.Entities.Support;
using NYC360.Domain.Dtos.Support;
using NYC360.Domain.Enums;

namespace NYC360.Application.Contracts.Persistence;

public interface ISupportTicketRepository
{
    Task AddAsync(SupportTicket ticket, CancellationToken ct);
    Task<SupportTicket?> GetByIdAsync(int ticketId, CancellationToken ct);
    Task<(List<SupportTicketDto>, int)> GetPagedAsync(SupportTicketStatus? status, int page, int pageSize, CancellationToken ct);
}