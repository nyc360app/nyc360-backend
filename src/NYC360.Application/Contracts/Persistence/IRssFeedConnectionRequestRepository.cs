using NYC360.Domain.Entities;
using NYC360.Domain.Enums;

namespace NYC360.Application.Contracts.Persistence;

public interface IRssFeedConnectionRequestRepository : IGenericRepository<RssFeedConnectionRequest>
{
    Task<IReadOnlyList<RssFeedConnectionRequest>> GetAllWithDetailsAsync(CancellationToken ct);
    Task<(IReadOnlyList<RssFeedConnectionRequest> Items, int Count)> GetPagedRequestsAsync(int pageNumber, int pageSize, RssConnectionStatus? status, CancellationToken ct);
}
