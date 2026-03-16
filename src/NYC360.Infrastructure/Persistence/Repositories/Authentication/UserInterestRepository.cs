using NYC360.Infrastructure.Persistence.DbContexts;
using NYC360.Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;
using NYC360.Domain.Enums;

namespace NYC360.Infrastructure.Persistence.Repositories.Authentication;

public class UserInterestRepository(ApplicationDbContext context) : IUserInterestRepository
{
    public async Task<IEnumerable<Category>> GetInterestsByUserIdAsync(int userId, CancellationToken cancellationToken)
    {
        // This query fetches the UserInterest records for the specified user ID,
        // then projects/selects only the Category enum value, and returns them as a list.
        return await context.UserInterests
            .Where(ui => ui.UserId == userId)
            .Select(ui => ui.Category)
            .ToListAsync(cancellationToken);
    }
}