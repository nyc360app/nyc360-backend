using NYC360.Domain.Entities.Forums;

namespace NYC360.Application.Contracts.Persistence;

public interface IForumQuestionRepository : IGenericRepository<ForumQuestion>
{
    Task<(IEnumerable<ForumQuestion>, int)> GetPagedQuestionsAsync(string forumSlug, int page, int pageSize, CancellationToken ct);
    Task<ForumQuestion?> GetQuestionByIdAsync(int id, CancellationToken ct);
}
