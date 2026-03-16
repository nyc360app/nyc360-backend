using FastEndpoints;
using MediatR;
using NYC360.Application.Features.Forums.Queries.GetDashboardForums;
using NYC360.Domain.Dtos.Forums;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Dashboard.Forums;

public class GetForumsEndpoint(IMediator mediator) : EndpointWithoutRequest<StandardResponse<List<ForumDto>>>
{
    public override void Configure()
    {
        Get("/forums-dashboard");
        Permissions(Domain.Constants.Permissions.Forums.View);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await mediator.Send(new GetDashboardForumsQuery(), ct);
        await Send.OkAsync(result, ct);
    }
}
