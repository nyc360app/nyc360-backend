using NYC360.Application.Features.Posts.Queries.GetHomeFeed;
using NYC360.Domain.Dtos.Pages;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.Homes;

public class GetAllHomeEndpoint(IMediator mediator) : EndpointWithoutRequest<StandardResponse<HomeFeedDto>>
{
    public override void Configure()
    {
        Get("/feeds/all/home");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var query = new GetHomeFeedQuery(userId.Value);
        var result = await mediator.Send(query, ct);

        await Send.OkAsync(result, ct);
    }
}