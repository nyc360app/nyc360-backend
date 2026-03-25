using MediatR;
using NYC360.Domain.Dtos.Communities;
using NYC360.Domain.Enums.Communities;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Communities.Dashboard.GetLeaderApplications;

public record GetCommunityLeaderApplicationsQuery(
    int Page,
    int PageSize,
    CommunityLeaderApplicationStatus? Status = CommunityLeaderApplicationStatus.Pending)
    : IRequest<StandardResponse<PagedResponse<CommunityLeaderApplicationAdminListItemDto>>>;
