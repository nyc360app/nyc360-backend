using NYC360.Domain.Entities.Posts;
using NYC360.Domain.Dtos.Posts;

namespace NYC360.Application.Contracts.Persistence;

public interface IPostFlagRepository
{
    Task AddAsync(PostFlag flag, CancellationToken ct);
    Task<PostFlag?> GetByIdAsync(int flagId, CancellationToken ct);
    Task<(List<PostFlagAdminDto>, int)> GetPendingFlagsPaginatedAsync(int page, int pageSize, CancellationToken ct);
    void Update(PostFlag flag);
    void Remove(PostFlag flag);
}