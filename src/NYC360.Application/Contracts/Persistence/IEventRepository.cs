using NYC360.Domain.Entities.Events;

namespace NYC360.Application.Contracts.Persistence;

public interface IEventRepository : IGenericRepository<Event>
{
    Task<Event?> GetEventWithDetailsAsync(int id, CancellationToken ct);
    Task<(IReadOnlyList<Event> Items, int TotalCount)> GetPagedEventsAsync(string? searchTerm, int category, int status, 
        DateTime? fromDate, DateTime? toDate, int? locationId, int pageNumber, int pageSize, CancellationToken ct);
    Task<Event?> GetEventWithTicketsAsync(int id, CancellationToken ct);
}