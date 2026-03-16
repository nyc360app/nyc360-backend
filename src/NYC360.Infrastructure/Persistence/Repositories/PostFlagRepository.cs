using NYC360.Infrastructure.Persistence.DbContexts;
using NYC360.Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;
using NYC360.Domain.Entities.Posts;
using NYC360.Domain.Dtos.Posts;
using NYC360.Domain.Enums;

namespace NYC360.Infrastructure.Persistence.Repositories;

public sealed class PostFlagRepository(ApplicationDbContext db) : IPostFlagRepository
{
    public async Task AddAsync(PostFlag flag, CancellationToken ct)
    {
        await db.PostFlags.AddAsync(flag, ct);
    }

    public async Task<PostFlag?> GetByIdAsync(int flagId, CancellationToken ct)
    {
        return await db.PostFlags.FirstOrDefaultAsync(f => f.Id == flagId, ct);
    }

    public async Task<(List<PostFlagAdminDto>, int)> GetPendingFlagsPaginatedAsync(
        int page, 
        int pageSize, 
        CancellationToken ct)
    {
        var baseQuery = db.PostFlags
            .AsNoTracking()
            .Where(f => f.Status == FlagStatus.Pending);

        var totalCount = await baseQuery.CountAsync(ct);

        // Complex projection query to pull data from Post and User entities
        var flags = await baseQuery
            .Include(f => f.Post) // Include Post for title/content
            .Include(f => f.User) // Include User for username
            .OrderBy(f => f.CreatedAt) // Oldest flags first
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(f => new PostFlagAdminDto(
                f.Id,
                f.PostId,
                f.Post.Title,
                f.Post.Content!.Length > 100 ? f.Post.Content.Substring(0, 100) + "..." : f.Post.Content, // Snippet logic
                f.UserId,
                f.User.UserName!,
                f.Reason,
                f.Details,
                f.CreatedAt,
                f.Status
            ))
            .ToListAsync(ct);

        return (flags, totalCount);
    }

    public void Update(PostFlag flag)
    {
        db.PostFlags.Update(flag);
    }

    public void Remove(PostFlag flag)
    {
        db.PostFlags.Remove(flag);
    }
}