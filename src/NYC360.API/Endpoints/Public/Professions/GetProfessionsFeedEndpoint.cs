using NYC360.Application.Features.Professions.Queries.GetFeed;
using NYC360.Domain.Dtos.Professions;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.Professions;

public class GetProfessionsFeedEndpoint(IMediator mediator) : EndpointWithoutRequest<StandardResponse<ProfessionsFeedDto>>
{
    public override void Configure()
    {
        Get("/professions/feed");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var query = new GetProfessionsFeedQuery(userId.Value);
        var result = await mediator.Send(query, ct);
        
        await Send.OkAsync(result, ct);
    }
}