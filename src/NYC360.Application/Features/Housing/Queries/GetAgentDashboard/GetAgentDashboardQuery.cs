using NYC360.Domain.Dtos.Housing;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Housing.Queries.GetAgentDashboard;

public record GetAgentDashboardQuery(int UserId) : IRequest<StandardResponse<AgentDashboardDto>>;
