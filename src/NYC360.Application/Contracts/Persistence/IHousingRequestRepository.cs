using NYC360.Domain.Entities.Housing;
using NYC360.Domain.Dtos.Housing;

namespace NYC360.Application.Contracts.Persistence;

public interface IHousingRequestRepository : IGenericRepository<HousingRequest>
{
    Task<AgentDashboardDto> GetAgentDashboardAsync(int userId, CancellationToken ct);
    Task<HousingRequest?> GetUserSpecificPostRequestAsync(int userId, int houseInfoId, CancellationToken ct);

    Task<(List<HousingRequest>, int)> GetAgentPagedRequestsAsync(
        int userId,
        int pageNumber, 
        int pageSize, 
        CancellationToken ct);

    Task<(List<HousingRequest>, int)> GetUserPagedRequestsAsync(
        int userId,
        int pageNumber,
        int pageSize,
        CancellationToken ct);
}