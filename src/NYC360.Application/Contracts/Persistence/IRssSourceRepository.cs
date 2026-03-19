using NYC360.Domain.Entities;
using NYC360.Domain.Enums;

namespace NYC360.Application.Contracts.Persistence;

public interface IRssSourceRepository
{
    Task AddAsync(RssFeedSource source, CancellationToken ct);
    Task<RssFeedSource?> GetByIdAsync(int id, CancellationToken ct);
    Task<List<RssFeedSource>> GetAllAsync(Category? category, CancellationToken ct);
    void Update(RssFeedSource source);
    void Update(List<RssFeedSource> sources);
    void Remove(RssFeedSource source);
    Task<bool> ExistsAsync(string url, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}
