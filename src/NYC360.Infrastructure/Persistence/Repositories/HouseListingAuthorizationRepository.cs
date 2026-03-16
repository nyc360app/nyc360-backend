using Microsoft.EntityFrameworkCore;
using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Entities.Housing;
using NYC360.Infrastructure.Persistence.DbContexts;

namespace NYC360.Infrastructure.Persistence.Repositories;

public class HouseListingAuthorizationRepository(ApplicationDbContext context) 
    : GenericRepository<HouseListingAuthorization>(context), IHouseListingAuthorizationRepository
{
    public async Task<List<HouseListingAuthorization>> GetByUserIdAsync(int userId, CancellationToken ct)
    {
        return await Context.HouseListingAuthorizations
            .Include(x => x.Attachments)
            .Include(x => x.Availabilities)
            .Where(x => x.UserId == userId)
            .ToListAsync(ct);
    }
}
