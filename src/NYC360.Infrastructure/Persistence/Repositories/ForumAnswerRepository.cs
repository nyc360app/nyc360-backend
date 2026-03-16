using Microsoft.EntityFrameworkCore;
using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Entities.Forums;
using NYC360.Infrastructure.Persistence.DbContexts;

namespace NYC360.Infrastructure.Persistence.Repositories;

public class ForumAnswerRepository(ApplicationDbContext context) 
    : GenericRepository<ForumAnswer>(context), IForumAnswerRepository
{
    public async Task<IEnumerable<ForumAnswer>> GetAnswersByQuestionIdAsync(int questionId, CancellationToken ct)
    {
        return await DbSet
            .Include(x => x.Author)
                .ThenInclude(u => u.User)
            .Where(x => x.QuestionId == questionId)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync(ct);
    }
}
