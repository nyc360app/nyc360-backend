using FastEndpoints;
using MediatR;
using NYC360.API.Extensions;
using NYC360.API.Models.RssSources;
using NYC360.Application.Contracts.Services;
using NYC360.Application.Features.RssSources.Queries.GetRequests;
using NYC360.Domain.Dtos.Rss;
using NYC360.Domain.Enums;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Dashboard.News;

public class GetNewsRssRequestsEndpoint(
    IMediator mediator,
    INewsAuthorizationService newsAuthorizationService)
    : Endpoint<GetRssConnectionRequestsListRequest, PagedResponse<RssConnectionRequestDto>>
{
    public override void Configure()
    {
        Get("/news-dashboard/rss/requests");
    }

    public override async Task HandleAsync(GetRssConnectionRequestsListRequest req, CancellationToken ct)
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

        var result = await mediator.Send(
            new GetRssConnectionRequestsQuery(req.PageNumber, req.PageSize, req.Status, Category.News), ct);

        await Send.OkAsync(result, ct);
    }
}
