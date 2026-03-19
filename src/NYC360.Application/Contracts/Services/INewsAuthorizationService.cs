using NYC360.Domain.Dtos.News;

namespace NYC360.Application.Contracts.Services;

public interface INewsAuthorizationService
{
    Task<NewsAccessDto?> GetAccessAsync(int userId, CancellationToken ct);
}
