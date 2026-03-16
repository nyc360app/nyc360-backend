using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.Housing;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Housing.Queries.GetAgentDashboard;

public class GetAgentDashboardQueryHandler(IHousingRequestRepository repository) 
    : IRequestHandler<GetAgentDashboardQuery, StandardResponse<AgentDashboardDto>>
{
    public async Task<StandardResponse<AgentDashboardDto>> Handle(GetAgentDashboardQuery request, CancellationToken ct)
    {
        var data = await repository.GetAgentDashboardAsync(request.UserId, ct);
        return StandardResponse<AgentDashboardDto>.Success(data);
    }
}
