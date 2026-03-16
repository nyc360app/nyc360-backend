using NYC360.Domain.Entities.Topics;
using NYC360.Domain.Enums;

namespace NYC360.Application.Contracts.Persistence;

public interface ITopicRepository : IGenericRepository<Topic>
{
    Task<IReadOnlyList<Topic>> GetTopicsByCategoryAsync(Category? category);
    Task<bool> IsTopicNameUnique(string name, Category? category);
    Task<(IReadOnlyList<Topic> Items, int TotalCount)> GetPagedTopicsAsync(int page, int pageSize, string? search, Category? category);
}
