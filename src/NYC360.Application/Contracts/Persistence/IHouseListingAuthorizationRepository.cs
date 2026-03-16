using NYC360.Domain.Entities.Housing;

namespace NYC360.Application.Contracts.Persistence;

public interface IHouseListingAuthorizationRepository : IGenericRepository<HouseListingAuthorization>
{
    Task<List<HouseListingAuthorization>> GetByUserIdAsync(int userId, CancellationToken ct);
}
