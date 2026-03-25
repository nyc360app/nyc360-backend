using MediatR;
using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.Communities;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Communities.Dashboard.GetLeaderApplications;

public class GetCommunityLeaderApplicationsQueryHandler(ICommunityRepository communityRepository)
    : IRequestHandler<GetCommunityLeaderApplicationsQuery, StandardResponse<PagedResponse<CommunityLeaderApplicationAdminListItemDto>>>
{
    public async Task<StandardResponse<PagedResponse<CommunityLeaderApplicationAdminListItemDto>>> Handle(
        GetCommunityLeaderApplicationsQuery request,
        CancellationToken ct)
    {
        var (applications, totalCount) = await communityRepository.GetLeaderApplicationsPaginatedAsync(
            request.Status,
            request.Page,
            request.PageSize,
            ct);

        var items = applications
            .Select(CommunityLeaderApplicationAdminDtoExtensions.MapListItem)
            .ToList();

        var paged = PagedResponse<CommunityLeaderApplicationAdminListItemDto>.Create(
            items,
            request.Page,
            request.PageSize,
            totalCount);

        return StandardResponse<PagedResponse<CommunityLeaderApplicationAdminListItemDto>>.Success(paged);
    }
}
