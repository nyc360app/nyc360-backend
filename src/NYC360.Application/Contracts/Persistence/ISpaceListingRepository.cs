using NYC360.Domain.Dtos.SpaceListings;
using NYC360.Domain.Enums;
using NYC360.Domain.Enums.SpaceListings;
using NYC360.Domain.Entities.SpaceListings;

namespace NYC360.Application.Contracts.Persistence;

public interface ISpaceListingRepository : IGenericRepository<SpaceListing>
{
    Task<SpaceListing?> GetByIdAsync(int id, CancellationToken ct);
    Task<SpaceListing?> GetByIdWithDetailsAsync(int id, CancellationToken ct);
    Task<List<SpaceListingListItemDto>> GetMyListingsAsync(int userId, int page, int pageSize, CancellationToken ct);
    Task<int> GetMyListingsCountAsync(int userId, CancellationToken ct);
    Task<List<SpaceListingListItemDto>> GetAdminListingsAsync(int page, int pageSize, Category? department, SpaceListingEntityType? entityType, SpaceListingStatus? status, string? search, CancellationToken ct);
    Task<int> GetAdminListingsCountAsync(Category? department, SpaceListingEntityType? entityType, SpaceListingStatus? status, string? search, CancellationToken ct);
    Task<SpaceListing?> FindDuplicateAsync(string nameNormalized, int? locationId, string? zipCode, string? websiteNormalized, string? phoneNormalized, string? emailNormalized, CancellationToken ct);
    Task<int> CountRecentSubmissionsAsync(int userId, DateTime sinceUtc, CancellationToken ct);
}
