using NYC360.Infrastructure.Persistence.DbContexts;
using NYC360.Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;
using NYC360.Domain.Entities.Forums;

namespace NYC360.Infrastructure.Persistence.Repositories;

public class ForumRepository(ApplicationDbContext context) : GenericRepository<Forum>(context), IForumRepository
{
    public async Task<List<Forum>> GetForumsAsync(CancellationToken ct)
    {
        return await DbSet
            .Include(x => x.Questions)
            .Where(x => x.IsActive)
            .OrderBy(x => x.Id)
            .ToListAsync(ct);
    }

    public async Task<List<Forum>> GetAllForumsAsync(CancellationToken ct)
    {
        return await DbSet
            .Include(x => x.Questions)
            .Include(x => x.Moderators)
                .ThenInclude(m => m.Moderator)
                    .ThenInclude(u => u.User)
            .OrderBy(x => x.Id)
            .ToListAsync(ct);
    }

    public async Task<Forum?> GetBySlugAsync(string slug, CancellationToken ct)
    {
        return await DbSet
            .FirstOrDefaultAsync(x => x.Slug == slug, ct);
    }

    public async Task<Forum?> GetByIdWithModeratorsAsync(int id, CancellationToken ct)
    {
        return await DbSet
            .Include(x => x.Moderators)
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }
}
