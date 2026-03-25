using FastEndpoints;
using MediatR;
using NYC360.Application.Features.Communities.Dashboard.GetLeaderApplicationDetails;
using NYC360.Domain.Dtos.Communities;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Communities.Dashboard;

public class GetLeaderApplicationDetailsEndpoint(IMediator mediator)
    : EndpointWithoutRequest<StandardResponse<CommunityLeaderApplicationAdminDetailsDto>>
{
    public override void Configure()
    {
        Get("/communities-dashboard/leader-applications/{ApplicationId}");
        Roles("Admin", "SuccessAdmin", "SuperAdmin");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var applicationId = Route<int>("ApplicationId");
        var query = new GetCommunityLeaderApplicationDetailsQuery(applicationId);
        var result = await mediator.Send(query, ct);

        await Send.OkAsync(result, ct);
    }
}
