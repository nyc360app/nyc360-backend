using NYC360.Infrastructure.Persistence.DbContexts;
using NYC360.Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;
using NYC360.Domain.Dtos.Tags;
using NYC360.Domain.Entities.Tags;
using NYC360.Domain.Enums.Tags;
using NYC360.Domain.Enums;

namespace NYC360.Infrastructure.Persistence.Repositories;

public class TagRepository(ApplicationDbContext context) : ITagRepository
{
    public async Task<Tag?> GetByIdAsync(int id)
    {
        return await context.Tags
            .Include(t => t.ChildTags)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<Tag?> GetByNameAsync(string name, CancellationToken ct)
    {
        return await context.Tags
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Name == name, ct);
    }

    public async Task<List<Tag>> GetByTypeAsync(TagType type)
    {
        return await context.Tags
            .Where(t => t.Type == type && t.ParentTagId == null)
            .Include(t => t.ChildTags)
            .ToListAsync();
    }

    public async Task<List<TagDto>> SearchTagsAsync(string term, Category? division, int limit, CancellationToken ct)
    {
        var tags = await context.Tags
            .AsNoTracking()
            .Include(t => t.ParentTag)
            .Where(t => !division.HasValue || t.Division == division)
            .Where(t => t.Name.Contains(term))
            .OrderBy(t => t.Name)
            .Take(limit)
            .ToListAsync(ct);
    
        return tags.Select(t => t.Map(includeChildren: false)).ToList();
    }

    public async Task<(List<Tag>, int)> GetPagedTagsAsync( string? search, TagType? type, 
        Category? division, int pageNumber, int pageSize, CancellationToken ct)
    {
        var query = context.Tags
            .Include(t => t.ParentTag)
            .Include(t => t.ChildTags)
            .AsQueryable();
    
        // Filtering
        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(t => t.Name.Contains(search));
    
        if (type.HasValue)
            query = query.Where(t => t.Type == type.Value);
    
        if (division.HasValue)
            query = query.Where(t => t.Division == division.Value);
    
        // Get Total Count before pagination
        var totalCount = await query.CountAsync(ct);
    
        // Apply Pagination
        var items = await query
            .OrderByDescending(t => t.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    
        return (items, totalCount);
    }
    public async Task AddAsync(Tag tag) => await context.Tags.AddAsync(tag);
    public void Update(Tag tag) => context.Tags.Update(tag);
    public void Delete(Tag tag) => context.Tags.Remove(tag);

    public async Task<bool> ExistsAsync(string name, int? parentId)
    {
        return await context.Tags.AnyAsync(t => t.Name == name && t.ParentTagId == parentId);
    }
    
    public async Task<bool> ExistsAtLevelAsync(string name, int? parentId, CancellationToken ct)
    {
        return await context.Tags
            .AnyAsync(x => x.Name.ToLower() == name.ToLower() && x.ParentTagId == parentId, ct);
    }
    
    public async Task<bool> UserHasTagForDivisionAsync(int userId, Category division, CancellationToken ct)
    {
        return await context.UserTags
            .Include(ut => ut.Tag)
            .Where(ut => ut.UserId == userId)
            .AnyAsync(ut => ut.Tag.Division == division, ct);
    }

    public async Task<List<Tag>> GetTagsByDivisionAsync(Category division, CancellationToken ct)
    {
        return await context.Tags
            .AsNoTracking()
            .Where(t => t.Division == division)
            .OrderBy(t => t.Name)
            .ToListAsync(ct);
    }

    public async Task AddUserTagAsync(UserTag userTag, CancellationToken ct)
    {
        await context.UserTags.AddAsync(userTag, ct);
    }

    public async Task RemoveUserTagAsync(int userId, int tagId, CancellationToken ct)
    {
        var userTag = await context.UserTags
            .FirstOrDefaultAsync(ut => ut.UserId == userId && ut.TagId == tagId, ct);

        if (userTag != null)
        {
            context.UserTags.Remove(userTag);
        }
    }

    public async Task<bool> UserHasTagAsync(int userId, string tag, CancellationToken ct)
    {
        return await context.UserTags
            .Include(ut => ut.Tag)
            .AnyAsync(ut => ut.UserId == userId && ut.Tag.Name == tag, ct);
    }
}