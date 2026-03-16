using NYC360.Domain.Entities.Housing;
using NYC360.Domain.Dtos.Housing;

namespace NYC360.Application.Contracts.Persistence;

public interface IHouseInfoRepository : IGenericRepository<HouseInfo>
{
    Task<(List<AgentListingDto>, int)> GetAgentPagedListingsAsync(int userId, int page, int pageSize, CancellationToken ct);
    Task<List<HouseInfo>> GetRecentHousingInfoAsync(int limit, CancellationToken ct);
    Task<HouseInfo?> GetHouseInfoByIdAsync(int houseId, CancellationToken ct);
    Task<HouseInfo?> GetHouseInfoByIdNoTrackingAsync(int houseId, CancellationToken ct);

    Task<(List<HouseInfo>, int)> GetPagedFeedAsync(
        int page, int pageSize, bool? isRenting, int? minPrice, int? maxPrice, int? locationId, string? search,
        CancellationToken ct);

    Task<(List<AgentListingDto>, int)> GetAdminPagedListingsAsync(int page, int pageSize, bool? isPublished, string? search, CancellationToken ct);

    Task<List<HouseInfo>> GetSimilarListingsAsync(HouseInfo current, int limit, CancellationToken ct);
    Task<List<HouseInfo>> SearchHousingAsync(string term, int limit, CancellationToken ct);
}