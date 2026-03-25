using FastEndpoints;
using MediatR;
using NYC360.Application.Features.Communities.Dashboard.GetLeaderApplications;
using NYC360.Domain.Dtos.Communities;
using NYC360.Domain.Enums.Communities;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Communities.Dashboard;

public class GetPendingLeaderApplicationsEndpoint(IMediator mediator)
    : EndpointWithoutRequest<StandardResponse<PagedResponse<CommunityLeaderApplicationAdminListItemDto>>>
{
    public override void Configure()
    {
        Get("/communities-dashboard/leader-applications/pending");
        Roles("Admin", "SuccessAdmin", "SuperAdmin");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var page = Query<int>("page", false);
        if (page == 0) page = 1;

        var pageSize = Query<int>("pageSize", false);
        if (pageSize == 0) pageSize = 10;

        var status = ParseStatusOrDefault(Query<string>("status", false), CommunityLeaderApplicationStatus.Pending);

        var query = new GetCommunityLeaderApplicationsQuery(page, pageSize, status);
        var result = await mediator.Send(query, ct);

        await Send.OkAsync(result, ct);
    }

    private static CommunityLeaderApplicationStatus ParseStatusOrDefault(
        string? rawStatus,
        CommunityLeaderApplicationStatus fallback)
    {
        if (string.IsNullOrWhiteSpace(rawStatus))
            return fallback;

        var normalized = rawStatus.Trim().Trim('"', '\'');

        if (Enum.TryParse<CommunityLeaderApplicationStatus>(normalized, true, out var parsedByName) &&
            Enum.IsDefined(typeof(CommunityLeaderApplicationStatus), parsedByName))
        {
            return parsedByName;
        }

        if (byte.TryParse(normalized, out var numeric) &&
            Enum.IsDefined(typeof(CommunityLeaderApplicationStatus), (int)numeric))
        {
            return (CommunityLeaderApplicationStatus)numeric;
        }

        return fallback;
    }
}
