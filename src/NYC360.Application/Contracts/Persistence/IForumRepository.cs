using NYC360.Domain.Entities.Forums;

namespace NYC360.Application.Contracts.Persistence;

public interface IForumRepository : IGenericRepository<Forum>
{
    Task<List<Forum>> GetForumsAsync(CancellationToken ct);
    Task<List<Forum>> GetAllForumsAsync(CancellationToken ct);
    Task<Forum?> GetBySlugAsync(string slug, CancellationToken ct);
    Task<Forum?> GetByIdWithModeratorsAsync(int id, CancellationToken ct);
}
