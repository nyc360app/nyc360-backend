using Microsoft.EntityFrameworkCore;
using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.SpaceListings;
using NYC360.Domain.Entities.SpaceListings;
using NYC360.Domain.Enums;
using NYC360.Domain.Enums.SpaceListings;
using NYC360.Infrastructure.Persistence.DbContexts;

namespace NYC360.Infrastructure.Persistence.Repositories;

public sealed class SpaceListingRepository(ApplicationDbContext context)
    : GenericRepository<SpaceListing>(context), ISpaceListingRepository
{
    public async Task<SpaceListing?> GetByIdAsync(int id, CancellationToken ct)
        => await context.SpaceListings.FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task<SpaceListing?> GetByIdWithDetailsAsync(int id, CancellationToken ct)
        => await context.SpaceListings
            .Include(x => x.Attachments)
            .Include(x => x.SocialLinks)
            .Include(x => x.Hours)
            .Include(x => x.ReviewEntries)
            .Include(x => x.Submitter)!.ThenInclude(p => p!.User)
            .FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task<List<SpaceListingListItemDto>> GetMyListingsAsync(int userId, int page, int pageSize, CancellationToken ct)
        => await context.SpaceListings.AsNoTracking()
            .Where(x => x.SubmitterUserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new SpaceListingListItemDto(
                x.Id,
                x.Name,
                x.Department,
                x.EntityType,
                x.Status,
                x.OwnershipStatus,
                x.SpaceItemId,
                x.CreatedAt,
                x.UpdatedAt))
            .ToListAsync(ct);

    public async Task<int> GetMyListingsCountAsync(int userId, CancellationToken ct)
        => await context.SpaceListings.AsNoTracking()
            .Where(x => x.SubmitterUserId == userId)
            .CountAsync(ct);

    public async Task<List<SpaceListingListItemDto>> GetAdminListingsAsync(
        int page,
        int pageSize,
        Category? department,
        SpaceListingEntityType? entityType,
        SpaceListingStatus? status,
        string? search,
        CancellationToken ct)
    {
        var query = BuildAdminQuery(department, entityType, status, search);

        return await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new SpaceListingListItemDto(
                x.Id,
                x.Name,
                x.Department,
                x.EntityType,
                x.Status,
                x.OwnershipStatus,
                x.SpaceItemId,
                x.CreatedAt,
                x.UpdatedAt))
            .ToListAsync(ct);
    }

    public async Task<int> GetAdminListingsCountAsync(
        Category? department,
        SpaceListingEntityType? entityType,
        SpaceListingStatus? status,
        string? search,
        CancellationToken ct)
        => await BuildAdminQuery(department, entityType, status, search).CountAsync(ct);

    public async Task<SpaceListing?> FindDuplicateAsync(
        string nameNormalized,
        int? locationId,
        string? zipCode,
        string? websiteNormalized,
        string? phoneNormalized,
        string? emailNormalized,
        CancellationToken ct)
    {
        var query = context.SpaceListings.AsNoTracking();

        var nameLocationMatch = query.Where(x => x.NameNormalized == nameNormalized);
        if (locationId.HasValue)
            nameLocationMatch = nameLocationMatch.Where(x => x.LocationId == locationId.Value);
        else if (!string.IsNullOrWhiteSpace(zipCode))
            nameLocationMatch = nameLocationMatch.Where(x => x.ZipCode == zipCode);

        IQueryable<SpaceListing> matches = nameLocationMatch;

        if (!string.IsNullOrWhiteSpace(websiteNormalized))
            matches = matches.Union(query.Where(x => x.WebsiteNormalized == websiteNormalized));
        if (!string.IsNullOrWhiteSpace(phoneNormalized))
            matches = matches.Union(query.Where(x => x.PhoneNumberNormalized == phoneNormalized));
        if (!string.IsNullOrWhiteSpace(emailNormalized))
            matches = matches.Union(query.Where(x => x.PublicEmailNormalized == emailNormalized));

        return await matches.FirstOrDefaultAsync(ct);
    }

    public async Task<int> CountRecentSubmissionsAsync(int userId, DateTime sinceUtc, CancellationToken ct)
        => await context.SpaceListings.AsNoTracking()
            .Where(x => x.SubmitterUserId == userId && x.CreatedAt >= sinceUtc)
            .CountAsync(ct);

    private IQueryable<SpaceListing> BuildAdminQuery(
        Category? department,
        SpaceListingEntityType? entityType,
        SpaceListingStatus? status,
        string? search)
    {
        var query = context.SpaceListings.AsNoTracking();

        if (department.HasValue)
            query = query.Where(x => x.Department == department.Value);
        if (entityType.HasValue)
            query = query.Where(x => x.EntityType == entityType.Value);
        if (status.HasValue)
            query = query.Where(x => x.Status == status.Value);
        else
            query = query.Where(x => x.Status == SpaceListingStatus.Pending
                                     || x.Status == SpaceListingStatus.UnderReview
                                     || x.Status == SpaceListingStatus.NeedsChanges);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var normalized = search.Trim().ToLowerInvariant();
            query = query.Where(x =>
                x.NameNormalized.Contains(normalized) ||
                (x.WebsiteNormalized != null && x.WebsiteNormalized.Contains(normalized)) ||
                (x.PhoneNumberNormalized != null && x.PhoneNumberNormalized.Contains(normalized)) ||
                (x.PublicEmailNormalized != null && x.PublicEmailNormalized.Contains(normalized)));
        }

        return query;
    }
}
