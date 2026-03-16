using NYC360.Domain.Entities;

namespace NYC360.Application.Contracts.Persistence;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken token, CancellationToken ct);
    Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken ct);
    Task RemoveAsync(int userId, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}