using Microsoft.EntityFrameworkCore;
using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Entities.Forums;
using NYC360.Infrastructure.Persistence.DbContexts;

namespace NYC360.Infrastructure.Persistence.Repositories;

public class ForumQuestionRepository(ApplicationDbContext context) 
    : GenericRepository<ForumQuestion>(context), IForumQuestionRepository
{
    public async Task<(IEnumerable<ForumQuestion>, int)> GetPagedQuestionsAsync(string forumSlug, int page, int pageSize, CancellationToken ct)
    {
        var query = DbSet
            .Include(x => x.Author)
                .ThenInclude(u => u.User)
            .Include(x => x.Answers)
            .Where(x => x.Forum.Slug == forumSlug);

        var total = await query.CountAsync(ct);
        
        var items = await query
            .OrderByDescending(x => x.IsPinned)
            .ThenByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, total);
    }

    public async Task<ForumQuestion?> GetQuestionByIdAsync(int id, CancellationToken ct)
    {
        return await DbSet
            .Include(x => x.Author)
                .ThenInclude(u => u.User)
            .Include(x => x.Forum)
            .Include(x => x.Answers)
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }
}
