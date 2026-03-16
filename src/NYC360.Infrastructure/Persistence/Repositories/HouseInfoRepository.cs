using NYC360.Infrastructure.Persistence.DbContexts;
using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Entities.Housing;
using NYC360.Domain.Dtos.Housing;
using Microsoft.EntityFrameworkCore;

namespace NYC360.Infrastructure.Persistence.Repositories;

public class HouseInfoRepository(ApplicationDbContext context) : GenericRepository<HouseInfo>(context), IHouseInfoRepository
{
    public async Task<(List<AgentListingDto>, int)> GetAgentPagedListingsAsync(int userId, int page, int pageSize, CancellationToken ct)
    {
        var query = Context.HouseInfos
            .Where(x => x.UserId == userId);

        var totalCount = await query.CountAsync(ct);

        var itemsRaw = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new
            {
                x.Id,
                ImageUrl = x.Attachments.OrderBy(a => a.Id).Select(a => a.Url).FirstOrDefault(),
                Price = x.StartingPrice,
                x.Neighborhood,
                HouseType = x.HouseType.ToString(),
                x.Bedrooms,
                x.Bathrooms,
                TotalInquiries = Context.HousingRequests.Count(r => r.HouseInfoId == x.Id),
                x.IsPublished,
                x.CreatedAt
            })
            .ToListAsync(ct);

        var items = itemsRaw.Select(x => new AgentListingDto(
            x.Id,
            x.ImageUrl,
            x.Price,
            x.Neighborhood,
            x.HouseType,
            x.Bedrooms,
            x.Bathrooms,
            x.TotalInquiries,
            x.IsPublished,
            x.CreatedAt,
            null
        )).ToList();

        return (items, totalCount);
    }

    public async Task<List<HouseInfo>> GetRecentHousingInfoAsync(int limit, CancellationToken ct)
    {
        return await Context.HouseInfos
            .AsNoTracking()
            .Where(p => p.IsPublished)
            .Include(p => p.Attachments)
            .OrderByDescending(x => x.Id)
            .Take(limit)
            .ToListAsync(ct);
    }

    public async Task<HouseInfo?> GetHouseInfoByIdAsync(int houseId, CancellationToken ct)
    {
        return await Context.HouseInfos
            .Include(hi => hi.Attachments)
            .Include(hi => hi.HouseListingAuthorization)
                .ThenInclude(a => a!.Attachments)
            .Include(hi => hi.HouseListingAuthorization)
                .ThenInclude(a => a!.Availabilities)
            .FirstOrDefaultAsync(hi => hi.Id == houseId, ct);
    }
    
    public async Task<HouseInfo?> GetHouseInfoByIdNoTrackingAsync(int houseId, CancellationToken ct)
    {
        return await Context.HouseInfos
            .AsNoTracking()
            .Include(hi => hi.User).ThenInclude(p => p.User)
            .Include(hi => hi.Attachments)
            .Include(hi => hi.HouseListingAuthorization)
                .ThenInclude(ha => ha.Attachments)
            .Include(hi => hi.HouseListingAuthorization)
                .ThenInclude(ha => ha.Availabilities)
            .FirstOrDefaultAsync(hi => hi.Id == houseId, ct);
    }
    
    public async Task<(List<HouseInfo>, int)> GetPagedFeedAsync(
        int page, int pageSize, bool? isRenting, int? minPrice, int? maxPrice, int? locationId, string? search, CancellationToken ct)
    {
        var query = Context.HouseInfos
            .Include(p => p.Attachments)
            .AsQueryable();

        // Filters
        if (isRenting.HasValue)
            query = query.Where(x => x.IsRenting == isRenting.Value);

        if (minPrice.HasValue)
            query = query.Where(x => x.StartingPrice >= minPrice.Value);

        if (maxPrice.HasValue)
            query = query.Where(x => x.StartingPrice <= maxPrice.Value);

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(x => x.Description.Contains(search));

        var totalCount = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(x => x.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, totalCount);
    }

    public async Task<(List<AgentListingDto>, int)> GetAdminPagedListingsAsync(int page, int pageSize, bool? isPublished, string? search, CancellationToken ct)
    {
        var query = Context.HouseInfos.AsQueryable();

        if (isPublished.HasValue)
            query = query.Where(x => x.IsPublished == isPublished.Value);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var isSearchInt = int.TryParse(search, out var searchInt);
            query = query.Where(x => 
                (isSearchInt && x.UserId == searchInt) ||
                x.Description.Contains(search) ||
                (x.User != null && (
                    x.User.Email!.Contains(search) ||
                    (x.User.User != null && x.User.User.UserName!.Contains(search))
                ))
            );
        }

        var totalCount = await query.CountAsync(ct);

        var houseInfos = await query
            .Include(x => x.User).ThenInclude(u => u.User)
            .Include(x => x.Attachments)
            .Include(x => x.HouseListingAuthorization)
                .ThenInclude(a => a!.Availabilities)
            .Include(x => x.HouseListingAuthorization)
                .ThenInclude(a => a!.Attachments)
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        var items = houseInfos.Select(x => new AgentListingDto(
            x.Id,
            x.Attachments.OrderBy(a => a.Id).Select(a => a.Url).FirstOrDefault(),
            x.StartingPrice,
            x.Neighborhood,
            x.HouseType.ToString(),
            x.Bedrooms,
            x.Bathrooms,
            Context.HousingRequests.Count(r => r.HouseInfoId == x.Id),
            x.IsPublished,
            x.CreatedAt,
            x.HouseListingAuthorization != null ? HouseListingAuthorizationDto.Map(x.HouseListingAuthorization) : null
        )).ToList();

        return (items, totalCount);
    }
    
    public async Task<List<HouseInfo>> GetSimilarListingsAsync(HouseInfo current, int limit, CancellationToken ct)
    {
        var similar = await Context.HouseInfos
            .AsNoTracking()
            .Include(p => p.Attachments)
            .Where(x => x.Id != current.Id) // Exclude current
            .Where(x => x.IsRenting == current.IsRenting) // Match category
            .Where(x => x.StartingPrice >= current.StartingPrice * 0.8 && 
                        x.StartingPrice <= current.StartingPrice * 1.2) // Within 20% price range
            .OrderByDescending(x => x.CreatedAt)
            .Take(limit)
            .ToListAsync(ct);
        
        //Fallback : Just Same Category (Rent/Sale) regardless of price
        if (similar.Count == 0)
        {
            similar = await DbSet
                .Include(p => p.Attachments)
                .OrderByDescending(x => x.CreatedAt)
                .Take(limit)
                .ToListAsync(ct);
        }
        
        return similar;
    }

    public async Task<List<HouseInfo>> SearchHousingAsync(string term, int limit, CancellationToken ct)
    {
        return await Context.HouseInfos
            .AsNoTracking()
            .Where(x => x.IsPublished && 
                       (x.Description.Contains(term) || 
                        x.Neighborhood.Contains(term) || 
                        x.Borough.Contains(term) ||
                        (x.FullAddress != null && x.FullAddress.Contains(term))))
            .Include(p => p.Attachments)
            .OrderByDescending(x => x.CreatedAt)
            .Take(limit)
            .ToListAsync(ct);
    }
}