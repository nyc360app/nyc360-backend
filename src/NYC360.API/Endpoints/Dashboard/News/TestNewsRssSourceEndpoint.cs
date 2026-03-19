using FastEndpoints;
using MediatR;
using NYC360.API.Extensions;
using NYC360.API.Models.News;
using NYC360.Application.Contracts.Services;
using NYC360.Application.Features.RssSources.Queries.Test;
using NYC360.Domain.Dtos.Rss;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Dashboard.News;

public class TestNewsRssSourceEndpoint(
    IMediator mediator,
    INewsAuthorizationService newsAuthorizationService)
    : Endpoint<TestNewsRssSourceRequest, StandardResponse<RssSourceDto>>
{
    public override void Configure()
    {
        Get("/news-dashboard/rss/test");
    }

    public override async Task HandleAsync(TestNewsRssSourceRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var access = await newsAuthorizationService.GetAccessAsync(userId.Value, ct);
        if (access == null || !access.CanConnectRss)
        {
            await Send.ForbiddenAsync(ct);
            return;
        }

        var result = await mediator.Send(new TestRssSourceQuery(req.Url), ct);
        await Send.OkAsync(result, ct);
    }
}
