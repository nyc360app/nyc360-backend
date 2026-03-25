using NYC360.Domain.Entities.Tags;

namespace NYC360.Application.Contracts.Persistence;

public interface IVerificationRepository
{
    Task<TagVerificationRequest?> GetByIdAsync(int id, CancellationToken ct);
    Task AddAsync(TagVerificationRequest request, CancellationToken ct);
    Task<bool> HasPendingRequestAsync(int userId, int tagId, CancellationToken ct);
    Task<bool> HasApprovedRequestAsync(int userId, int tagId, CancellationToken ct);
    Task<bool> HasApprovedIdentityRequestAsync(int userId, CancellationToken ct);
    Task<bool> UserHasSpecificTagAsync(int userId, int tagId, CancellationToken ct);
    Task<(List<TagVerificationRequest>, int)> GetPagedPendingRequestsAsync(int page, int pageSize, CancellationToken ct);
}
