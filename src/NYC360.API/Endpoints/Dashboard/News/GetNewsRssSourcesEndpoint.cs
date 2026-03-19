using FastEndpoints;
using MediatR;
using NYC360.API.Extensions;
using NYC360.Application.Contracts.Services;
using NYC360.Application.Features.RssSources.Queries.GetAll;
using NYC360.Domain.Dtos.Rss;
using NYC360.Domain.Enums;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Dashboard.News;

public class GetNewsRssSourcesEndpoint(
    IMediator mediator,
    INewsAuthorizationService newsAuthorizationService)
    : EndpointWithoutRequest<StandardResponse<List<RssSourceDto>>>
{
    public override void Configure()
    {
        Get("/news-dashboard/rss/sources");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var access = await newsAuthorizationService.GetAccessAsync(userId.Value, ct);
        if (access == null || !access.CanReviewRssRequests)
        {
            await Send.ForbiddenAsync(ct);
            return;
        }

        var result = await mediator.Send(new RssSourceGetAllQuery(Category.News), ct);
        await Send.OkAsync(result, ct);
    }
}
