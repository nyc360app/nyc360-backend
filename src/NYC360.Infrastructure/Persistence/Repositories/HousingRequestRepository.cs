using NYC360.Infrastructure.Persistence.DbContexts;
using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Entities.Housing;
using NYC360.Domain.Dtos.Housing;
using NYC360.Domain.Wrappers;
using Microsoft.EntityFrameworkCore;

namespace NYC360.Infrastructure.Persistence.Repositories;

public class HousingRequestRepository(ApplicationDbContext context) 
    : GenericRepository<HousingRequest>(context), IHousingRequestRepository
{
    public async Task<AgentDashboardDto> GetAgentDashboardAsync(int userId, CancellationToken ct)
    {
        var now = DateTime.UtcNow;
        var thirtyDaysAgo = now.AddDays(-30);
        var startOfCurrentMonth = new DateTime(now.Year, now.Month, 1);
        var startOfLastMonth = startOfCurrentMonth.AddMonths(-1);
        var endOfLastMonth = startOfCurrentMonth.AddTicks(-1);
        var today = now.Date;

        var agentRequests = Context.HousingRequests
            .Where(x => x.HouseInfo!.UserId == userId);

        // Stats
        var totalCount = await agentRequests.CountAsync(ct);
        
        var currentMonthCount = await agentRequests
            .CountAsync(x => x.CreatedAt >= startOfCurrentMonth, ct);
        
        var lastMonthCount = await agentRequests
            .CountAsync(x => x.CreatedAt >= startOfLastMonth && x.CreatedAt <= endOfLastMonth, ct);

        double monthlyChange = 0;
        if (lastMonthCount > 0)
        {
            monthlyChange = ((double)(currentMonthCount - lastMonthCount) / lastMonthCount) * 100;
        }
        else if (currentMonthCount > 0)
        {
            monthlyChange = 100;
        }

        var todayCount = await agentRequests
            .CountAsync(x => x.CreatedAt >= today, ct);

        var lastInquiryDate = await agentRequests
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => (DateTime?)x.CreatedAt)
            .FirstOrDefaultAsync(ct);

        int? lastUpdatedMinutesAgo = lastInquiryDate.HasValue 
            ? (int)(now - lastInquiryDate.Value).TotalMinutes 
            : null;

        var rentRequests = await agentRequests.CountAsync(x => x.HouseInfo!.IsRenting, ct);
        var saleRequests = totalCount - rentRequests;

        double rentPercentage = totalCount > 0 ? ((double)rentRequests / totalCount) * 100 : 0;
        double salePercentage = totalCount > 0 ? ((double)saleRequests / totalCount) * 100 : 0;

        // Trends (last 30 days)
        var trendsRaw = await agentRequests
            .Where(x => x.CreatedAt >= thirtyDaysAgo)
            .GroupBy(x => x.CreatedAt.Date)
            .Select(g => new { Date = g.Key, Count = g.Count() })
            .OrderBy(x => x.Date)
            .ToListAsync(ct);

        var trends = trendsRaw
            .Select(x => new AgentDashboardTrendPoint(x.Date, x.Count))
            .ToList();

        // Top Neighborhoods
        var topNeighborhoodsRaw = await agentRequests
            .GroupBy(x => x.HouseInfo!.Neighborhood)
            .Select(g => new { Neighborhood = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(5)
            .ToListAsync(ct);

        var topNeighborhoods = topNeighborhoodsRaw
            .Select(x => new AgentDashboardNeighborhoodStats(x.Neighborhood, x.Count))
            .ToList();

        // Recent Inquiries (10)
        var recentRaw = await agentRequests
            .Include(x => x.User)
            .Include(x => x.HouseInfo)
            .OrderByDescending(x => x.CreatedAt)
            .Take(10)
            .ToListAsync(ct);

        var recentItems = recentRaw.Select(x => new AgentDashboardRecentInquiry(
            x.Id,
            new AgentDashboardUser(
                x.User != null ? x.User.GetFullName() : (x.Name ?? "Anonymous"),
                x.User != null ? (x.User.Email ?? "") : (x.Email ?? "")
            ),
            x.HouseInfo!.IsRenting ? AgentDashboardRequestType.Rent : AgentDashboardRequestType.Sale,
            x.HouseInfo.Neighborhood,
            x.HouseInfo.HouseType.ToString(),
            x.Status.ToString()
        )).ToList();

        var recentPaged = PagedResponse<AgentDashboardRecentInquiry>.Create(recentItems, 1, 10, totalCount);

        return new AgentDashboardDto(
            new AgentDashboardStats(
                new AgentDashboardStatItem(totalCount, monthlyChange),
                new AgentDashboardNewInquiries(todayCount, lastUpdatedMinutesAgo),
                new AgentDashboardTypeBreakdown(rentPercentage, salePercentage)
            ),
            trends,
            topNeighborhoods,
            recentPaged
        );
    }

    public async Task<HousingRequest?> GetUserSpecificPostRequestAsync(int userId, int houseInfoId, CancellationToken ct)
    {
        return await Context.HousingRequests
            .AsNoTracking()
            .Include(x => x.HouseInfo)
                .ThenInclude(x => x.HouseListingAuthorization)
            .Include(x => x.User)
                .ThenInclude(x => x.User)
            .FirstOrDefaultAsync(x => x.HouseInfoId == houseInfoId && x.UserId == userId, ct);
    }

    public async Task<(List<HousingRequest>, int)> GetAgentPagedRequestsAsync(
        int userId,
        int pageNumber, 
        int pageSize, 
        CancellationToken ct)
    {
        var query = Context.HousingRequests
                .Where(x => x.HouseInfo!.UserId == userId)
                .Include(x => x.User).ThenInclude(x => x.User)
                .Include(x => x.HouseInfo)
                .OrderByDescending(x => x.Id);
        
        // 5. Get Total Count of requests owned by this user
        var totalCount = await query.CountAsync(ct);

        // 6. Apply Paging
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, totalCount);
    }
    
    public async Task<(List<HousingRequest>, int)> GetUserPagedRequestsAsync(
        int userId,
        int pageNumber, 
        int pageSize, 
        CancellationToken ct)
    {
        var query = Context.HousingRequests
            .Where(x => x.UserId == userId)
            .Include(x => x.User).ThenInclude(x => x.User)
            .Include(x => x.HouseInfo)
            .OrderByDescending(x => x.Id);
        
        // 5. Get Total Count of requests owned by this user
        var totalCount = await query.CountAsync(ct);

        // 6. Apply Paging
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, totalCount);
    }
}