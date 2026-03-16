using NYC360.Infrastructure.Persistence.DbContexts;
using NYC360.Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;
using NYC360.Domain.Entities.Topics;
using NYC360.Domain.Enums;

namespace NYC360.Infrastructure.Persistence.Repositories;

public class TopicRepository(ApplicationDbContext dbContext) 
    : GenericRepository<Topic>(dbContext), ITopicRepository
{
    public async Task<IReadOnlyList<Topic>> GetTopicsByCategoryAsync(Category? category)
    {
        return await dbContext.Topics
            .Where(t => t.Category == category)
            .ToListAsync();
    }

    public async Task<bool> IsTopicNameUnique(string name, Category? category)
    {
        return !await dbContext.Topics
            .AnyAsync(t => t.Name == name && t.Category == category);
    }

    public async Task<(IReadOnlyList<Topic> Items, int TotalCount)> GetPagedTopicsAsync(int page, int pageSize, string? search, Category? category)
    {
        var query = dbContext.Topics.AsQueryable();

        if (category.HasValue)
            query = query.Where(t => t.Category == category.Value);

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(t => t.Name.Contains(search));

        var total = await query.CountAsync();
        var items = await query.OrderBy(t => t.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, total);
    }
}
