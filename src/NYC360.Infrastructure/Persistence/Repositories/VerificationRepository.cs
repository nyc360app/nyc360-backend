using NYC360.Infrastructure.Persistence.DbContexts;
using NYC360.Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;
using NYC360.Domain.Entities.Tags;
using NYC360.Domain.Enums;
using NYC360.Domain.Enums.Tags;

namespace NYC360.Infrastructure.Persistence.Repositories;

public class VerificationRepository(ApplicationDbContext context) : IVerificationRepository
{
    public async Task<TagVerificationRequest?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await context.TagVerificationRequests.FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task AddAsync(TagVerificationRequest request, CancellationToken ct)
    {
        await context.TagVerificationRequests.AddAsync(request, ct);
    }

    public async Task<bool> HasPendingRequestAsync(int userId, int tagId, CancellationToken ct)
    {
        return await context.Set<TagVerificationRequest>()
            .AnyAsync(x => x.UserId == userId && 
                           x.TargetTagId == tagId && 
                           x.Status == VerificationStatus.Pending, ct);
    }

    public async Task<bool> HasApprovedRequestAsync(int userId, int tagId, CancellationToken ct)
    {
        return await context.Set<TagVerificationRequest>()
            .AnyAsync(x => x.UserId == userId &&
                           x.TargetTagId == tagId &&
                           x.Status == VerificationStatus.Approved, ct);
    }

    public async Task<bool> HasApprovedIdentityRequestAsync(int userId, CancellationToken ct)
    {
        return await context.Set<TagVerificationRequest>()
            .AnyAsync(x => x.UserId == userId &&
                           x.Status == VerificationStatus.Approved &&
                           x.TargetTag != null &&
                           x.TargetTag.Type == TagType.Identity, ct);
    }

    public async Task<bool> UserHasSpecificTagAsync(int userId, int tagId, CancellationToken ct)
    {
        return await context.UserTags.AnyAsync(x => x.UserId == userId && x.TagId == tagId, ct);
    }

    public async Task<(List<TagVerificationRequest>, int)> GetPagedPendingRequestsAsync(int page, int pageSize, CancellationToken ct)
    {
        var query = context.TagVerificationRequests
            .Include(x => x.User).ThenInclude(u => u.User)
            .Include(x => x.TargetTag)
            .Include(x => x.Documents)
            .Where(x => x.Status == VerificationStatus.Pending);

        var totalCount = await query.CountAsync(ct);
    
        var items = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, totalCount);
    }
}
