using NYC360.Infrastructure.Persistence.DbContexts;
using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Entities.Communities;
using NYC360.Domain.Enums.Communities;
using NYC360.Domain.Dtos.Communities;
using Microsoft.EntityFrameworkCore;

namespace NYC360.Infrastructure.Persistence.Repositories;

public sealed class CommunityRepository(ApplicationDbContext context) : ICommunityRepository
{
    public async Task AddAsync(Community community, CancellationToken ct)
    {
        await context.Communities.AddAsync(community, ct);
    }

    public async Task<Community?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await context.Communities
            .Include(c => c.Location)
            .Include(c => c.Members).ThenInclude(cm => cm.User)
            .FirstOrDefaultAsync(c => c.Id == id, ct);
    }

    public async Task<Community?> GetBySlugAsync(string slug, CancellationToken ct)
    {
        return await context.Communities
            .AsNoTracking()
            .Include(c => c.Location)
            .Include(c => c.Members)
                .ThenInclude(m => m.User)
                    .ThenInclude(u => u!.User)
            .FirstOrDefaultAsync(c => c.Slug == slug, ct);
    }

    public async Task<List<Community>> GetAllPaginatedAsync(int page, int itemsPerPage, CancellationToken ct)
    {
        return await context.Communities
            .OrderByDescending(c => c.Id)
            .Skip((page - 1) * itemsPerPage)
            .Take(itemsPerPage)
            .ToListAsync(ct);
    }

    public async Task<(List<Community>, int)> SearchCommunitiesAsync(int? userId, string? searchTerm, CommunityType? type, int? locationId, int page, int pageSize, CancellationToken ct)
    {
        var query = context.Communities
            .AsNoTracking()
            .Where(c => c.IsActive && !c.IsPrivate)
            .AsQueryable();

        if (userId.HasValue)
            query = query.Where(c => c.Members.All(m => m.UserId != userId.Value));

        if (!string.IsNullOrWhiteSpace(searchTerm))
            query = query.Where(c => c.Name.Contains(searchTerm) || c.Description.Contains(searchTerm));

        if (type.HasValue)
            query = query.Where(c => c.Type == type);

        if (locationId.HasValue)
            query = query.Where(c => c.LocationId == locationId);

        var total = await query.CountAsync(ct);
        var items = await query
            .Include(c => c.Members) // To get member count
            .OrderByDescending(c => c.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, total);
    }

    public async Task<List<CommunityDiscoveryDto>> SearchCommunitiesAsync(string term, int limit, CancellationToken ct)
    {
        return await context.Communities
            .AsNoTracking()
            .Where(c => c.Name.Contains(term) || c.Description.Contains(term))
            .OrderByDescending(c => c.MemberCount) // Prioritize popular communities
            .Take(limit)
            .Select(c => CommunityDiscoveryDto.Map(c))
            .ToListAsync(ct);
    }

    public async Task<(List<Community>, int)> SearchUserCommunitiesAsync(int userId, string? searchTerm, CommunityType? type, int? locationId, int page,
        int pageSize, CancellationToken ct)
    {
        var query = context.Communities
            .Where(c => c.IsActive)
            .Where(c => c.Members.Any(m => m.UserId == userId))
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
            query = query.Where(c => c.Name.Contains(searchTerm) || c.Description.Contains(searchTerm));

        if (type.HasValue)
            query = query.Where(c => c.Type == type);

        if (locationId.HasValue)
            query = query.Where(c => c.LocationId == locationId);

        var total = await query.CountAsync(ct);
        var items = await query
            .Include(c => c.Members) // To get member count
            .OrderByDescending(c => c.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, total);
    }

    public async Task<bool> IsNameAvailableAsync(string name, CancellationToken ct)
    {
        return await context.Communities.AnyAsync(c => c.Name == name, ct);
    }

    public async Task<bool> ExistsByNameTypeAndLocationAsync(string name, CommunityType? type, int? locationId, CancellationToken ct)
    {
        var normalizedName = name.Trim().ToLower();

        return await context.Communities
            .AnyAsync(c =>
                c.Name.ToLower() == normalizedName &&
                c.Type == type &&
                c.LocationId == locationId,
                ct);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken ct)
    {
        return await context.Communities.AnyAsync(c => c.Id == id, ct);
    }

    public async Task<bool> SlugExistsAsync(string slug, CancellationToken ct)
    {
        return await context.Communities.AnyAsync(c => c.Slug == slug, ct);
    }

    public async Task<CommunityMember?> GetMemberAsync(int communityId, int userId, CancellationToken ct)
    {
        return await context.CommunityMembers
            .FirstOrDefaultAsync(cm => cm.CommunityId == communityId && cm.UserId == userId, ct);
    }

    public async Task<int> GetMemberCountAsync(int communityId, CancellationToken ct)
    {
        return await context.CommunityMembers.CountAsync(cm => cm.CommunityId == communityId, ct);
    }

    public async Task<bool> IsMemberAsync(int communityId, int userId, CancellationToken ct)
    {
        return await context.CommunityMembers.AnyAsync(cm => cm.CommunityId == communityId && cm.UserId == userId, ct);
    }

    public async Task AddMemberAsync(CommunityMember member, CancellationToken ct)
    {
        await context.CommunityMembers.AddAsync(member, ct);
    }

    public void RemoveMember(CommunityMember member)
    {
        context.CommunityMembers.Remove(member);
    }

    public async Task<List<CommunityMember>> GetMembersPaginatedAsync(int communityId, int page, int itemsPerPage, CancellationToken ct)
    {
        return await context.CommunityMembers
            .Where(cm => cm.CommunityId == communityId)
            .AsNoTracking()
            .Include(cm => cm.User)
            .OrderByDescending(cm => cm.Id)
            .Skip((page - 1) * itemsPerPage)
            .Take(itemsPerPage)
            .ToListAsync(ct);
    }

    public void Update(Community community)
    {
        context.Communities.Update(community);
    }

    public void Remove(Community community)
    {
        // 1. Handle Members (often tracked by GetByIdAsync)
        if (community.Members != null && community.Members.Any())
        {
            context.CommunityMembers.RemoveRange(community.Members);
        }
        else
        {
            var members = context.CommunityMembers.Where(m => m.CommunityId == community.Id);
            context.CommunityMembers.RemoveRange(members);
        }

        // 2. Handle Join Requests
        var joinRequests = context.CommunityJoinRequests.Where(r => r.CommunityId == community.Id);
        context.CommunityJoinRequests.RemoveRange(joinRequests);

        // 3. Handle Posts
        var posts = context.Posts.Where(p => p.CommunityId == community.Id);
        context.Posts.RemoveRange(posts);

        // 4. Remove the community itself
        context.Communities.Remove(community);
    }

    public async Task<(List<CommunityMember>, int)> SearchMembersAsync(int communityId, string searchTerm, int page, int pageSize, CancellationToken ct)
    {
        var query = context.CommunityMembers
            .AsNoTracking()
            .Include(cm => cm.User)
            .ThenInclude(up => up!.User)
            .Where(cm => cm.CommunityId == communityId);

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.Trim().ToLower();
            query = query.Where(cm =>
                (cm.User != null && cm.User.FirstName != null && cm.User.FirstName.ToLower().Contains(term)) ||
                (cm.User != null && cm.User.LastName != null && cm.User.LastName.ToLower().Contains(term)) ||
                (cm.User != null && cm.User.User != null && cm.User.User.UserName != null && cm.User.User.UserName.ToLower().Contains(term))
            );
        }

        var totalCount = await query.CountAsync(ct);

        var members = await query
            .OrderBy(cm => cm.User!.FirstName)
            .ThenBy(cm => cm.User!.LastName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (members, totalCount);
    }
    
    public async Task<List<int>> GetJoinedCommunityIdsAsync(int userId, CancellationToken ct)
    {
        return await context.CommunityMembers
            .Where(cm => cm.UserId == userId)
            .Select(cm => cm.CommunityId)
            .ToListAsync(ct);
    }

    public async Task<List<Community>> GetSuggestionsAsync(int userId, int count, CancellationToken ct)
    {
        return await context.Communities
            .Where(c => c.IsActive && !c.IsPrivate)
            .Where(c => !context.CommunityMembers.Any(cm => cm.UserId == userId && cm.CommunityId == c.Id))
            .OrderByDescending(c => c.Members.Count) // Suggest popular ones
            .Take(count)
            .Include(c => c.Members)
            .ToListAsync(ct);
    }
    
    public async Task<CommunityJoinRequest?> GetJoinRequestAsync(int communityId, int userId, CancellationToken ct)
    {
        return await context.CommunityJoinRequests
            .FirstOrDefaultAsync(r => r.CommunityId == communityId && r.UserId == userId, ct);
    }

    public async Task AddJoinRequestAsync(CommunityJoinRequest request, CancellationToken ct)
    {
        await context.CommunityJoinRequests.AddAsync(request, ct);
    }
    public async Task<List<CommunityJoinRequest>> GetPendingRequestsAsync(int communityId, CancellationToken ct)
    {
        return await context.CommunityJoinRequests
            .Include(r => r.User)
            .Where(r => r.CommunityId == communityId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(ct);
    }

    public async Task<List<CommunityJoinRequest>> GetUserJoinRequestsAsync(int userId, CancellationToken ct)
    {
        return await context.CommunityJoinRequests
            .Include(r => r.Community)
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(ct);
    }

    public void RemoveJoinRequest(CommunityJoinRequest request)
    {
        context.CommunityJoinRequests.Remove(request);
    }

    // Disband request methods
    public async Task AddDisbandRequestAsync(CommunityDisbandRequest request, CancellationToken ct)
    {
        await context.CommunityDisbandRequests.AddAsync(request, ct);
    }

    public async Task AddLeaderApplicationAsync(CommunityLeaderApplication application, CancellationToken ct)
    {
        await context.CommunityLeaderApplications.AddAsync(application, ct);
    }

    public async Task<CommunityDisbandRequest?> GetDisbandRequestByIdAsync(int id, CancellationToken ct)
    {
        return await context.CommunityDisbandRequests
            .Include(r => r.Community)
            .Include(r => r.RequestedByUser)
            .Include(r => r.ProcessedByUser)
            .FirstOrDefaultAsync(r => r.Id == id, ct);
    }

    public async Task<CommunityDisbandRequest?> GetPendingDisbandRequestAsync(int communityId, CancellationToken ct)
    {
        return await context.CommunityDisbandRequests
            .FirstOrDefaultAsync(r => r.CommunityId == communityId && r.Status == DisbandRequestStatus.Pending, ct);
    }

    public async Task<List<CommunityDisbandRequest>> GetUserDisbandRequestsAsync(int userId, CancellationToken ct)
    {
        return await context.CommunityDisbandRequests
            .Include(r => r.Community)
            .Where(r => r.RequestedByUserId == userId)
            .OrderByDescending(r => r.RequestedAt)
            .ToListAsync(ct);
    }

    public async Task<(List<CommunityDisbandRequest>, int)> GetDisbandRequestsPaginatedAsync(
        DisbandRequestStatus? status, int page, int pageSize, CancellationToken ct)
    {
        var query = context.CommunityDisbandRequests
            .Include(r => r.Community)
            .Include(r => r.RequestedByUser)
            .Include(r => r.ProcessedByUser)
            .AsQueryable();

        if (status.HasValue)
            query = query.Where(r => r.Status == status.Value);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(r => r.RequestedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, total);
    }

    public async Task<bool> HasPendingDisbandRequestAsync(int communityId, CancellationToken ct)
    {
        return await context.CommunityDisbandRequests
            .AnyAsync(r => r.CommunityId == communityId && r.Status == DisbandRequestStatus.Pending, ct);
    }

    public async Task<bool> HasPendingLeaderApplicationAsync(int userId, CancellationToken ct)
    {
        return await context.CommunityLeaderApplications
            .AnyAsync(x => x.UserId == userId && x.Status == CommunityLeaderApplicationStatus.Pending, ct);
    }

    public void UpdateDisbandRequest(CommunityDisbandRequest request)
    {
        context.CommunityDisbandRequests.Update(request);
    }

    // Admin dashboard methods
    public async Task<(List<Community>, int)> GetAllCommunitiesPaginatedAsync(
        string? searchTerm, CommunityType? type, int? locationId, bool? hasDisbandRequest,
        int page, int pageSize, CancellationToken ct)
    {
        var query = context.Communities
            .Include(c => c.Members)
            .Include(c => c.DisbandRequests)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
            query = query.Where(c => c.Name.Contains(searchTerm) || c.Description.Contains(searchTerm));

        if (type.HasValue)
            query = query.Where(c => c.Type == type);

        if (locationId.HasValue)
            query = query.Where(c => c.LocationId == locationId);

        if (hasDisbandRequest.HasValue && hasDisbandRequest.Value)
        {
            query = query.Where(c => context.CommunityDisbandRequests
                .Any(r => r.CommunityId == c.Id && r.Status == DisbandRequestStatus.Pending));
        }

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(c => c.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, total);
    }

    public async Task<List<CommunityMember>> GetLeadersAsync(int communityId, CancellationToken ct)
    {
        return await context.CommunityMembers
            .Include(cm => cm.User)
            .Where(cm => cm.CommunityId == communityId && cm.Role == CommunityRole.Leader)
            .OrderBy(cm => cm.JoinedAt)
            .ToListAsync(ct);
    }

    public async Task<int> GetLeaderCountAsync(int communityId, CancellationToken ct)
    {
        return await context.CommunityMembers
            .CountAsync(cm => cm.CommunityId == communityId && cm.Role == CommunityRole.Leader, ct);
    }

    public async Task<int> GetModeratorCountAsync(int communityId, CancellationToken ct)
    {
        return await context.CommunityMembers
            .CountAsync(cm => cm.CommunityId == communityId && cm.Role == CommunityRole.Moderator, ct);
    }
}
