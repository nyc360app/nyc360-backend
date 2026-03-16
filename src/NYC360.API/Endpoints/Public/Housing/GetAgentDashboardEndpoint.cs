using NYC360.Application.Features.Housing.Queries.GetAgentDashboard;
using NYC360.Domain.Dtos.Housing;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.Housing;

public class GetAgentDashboardEndpoint(IMediator mediator) 
    : EndpointWithoutRequest<StandardResponse<AgentDashboardDto>>
{
    public override void Configure()
    {
        Get("/housing/agent/dashboard");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            // For now, since restrictions are to be set later, maybe we return unauthorized 
            // or we use a hardcoded/test ID? 
            // The user said "focus on logic", usually logic implies it works for the logged in agent.
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var query = new GetAgentDashboardQuery(userId.Value);
        var result = await mediator.Send(query, ct);

        await Send.OkAsync(result, ct);
    }
}
