using NYC360.Domain.Entities.Forums;

namespace NYC360.Application.Contracts.Persistence;

public interface IForumAnswerRepository : IGenericRepository<ForumAnswer>
{
    Task<IEnumerable<ForumAnswer>> GetAnswersByQuestionIdAsync(int questionId, CancellationToken ct);
}
