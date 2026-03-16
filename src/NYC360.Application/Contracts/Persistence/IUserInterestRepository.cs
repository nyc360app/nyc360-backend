using NYC360.Domain.Enums;

namespace NYC360.Application.Contracts.Persistence;

public interface IUserInterestRepository
{
    Task<IEnumerable<Category>> GetInterestsByUserIdAsync(int userId, CancellationToken cancellationToken);
}