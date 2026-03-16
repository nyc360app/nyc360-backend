using NYC360.Domain.Dtos.Tags;
using NYC360.Domain.Entities.Tags;
using NYC360.Domain.Enums.Tags;
using NYC360.Domain.Enums;

namespace NYC360.Application.Contracts.Persistence;

public interface ITagRepository
{
    Task<Tag?> GetByIdAsync(int id);
    Task<Tag?> GetByNameAsync(string name, CancellationToken ct);
    Task<List<Tag>> GetByTypeAsync(TagType type);
    Task<List<TagDto>> SearchTagsAsync(string term, Category? division, int limit, CancellationToken ct);
    Task<(List<Tag>, int)> GetPagedTagsAsync(string? search, TagType? type, Category? division, int pageNumber, int pageSize, CancellationToken ct);
    Task AddAsync(Tag tag);
    void Update(Tag tag);
    void Delete(Tag tag);
    Task<bool> ExistsAsync(string name, int? parentId);
    Task<bool> ExistsAtLevelAsync(string name, int? parentId, CancellationToken ct);
    Task<bool> UserHasTagForDivisionAsync(int userId, Category division, CancellationToken ct);
    Task<List<Tag>> GetTagsByDivisionAsync(Category division, CancellationToken ct);
    Task AddUserTagAsync(UserTag userTag, CancellationToken ct);
    Task RemoveUserTagAsync(int userId, int tagId, CancellationToken ct);
    Task<bool> UserHasTagAsync(int userId, string tag, CancellationToken ct);
}