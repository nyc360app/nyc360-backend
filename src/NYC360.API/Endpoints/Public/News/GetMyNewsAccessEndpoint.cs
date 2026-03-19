using FastEndpoints;
using MediatR;
using NYC360.API.Extensions;
using NYC360.Application.Features.News.Queries.GetMyAccess;
using NYC360.Domain.Dtos.News;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Public.News;

public class GetMyNewsAccessEndpoint(IMediator mediator) : EndpointWithoutRequest<StandardResponse<NewsAccessDto>>
{
    public override void Configure()
    {
        Get("/news/me/access");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var result = await mediator.Send(new GetMyNewsAccessQuery(userId.Value), ct);
        await Send.OkAsync(result, ct);
    }
}
