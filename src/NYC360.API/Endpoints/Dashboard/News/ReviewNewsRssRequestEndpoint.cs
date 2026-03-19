using FastEndpoints;
using MediatR;
using NYC360.API.Extensions;
using NYC360.API.Models.RssSources;
using NYC360.Application.Contracts.Services;
using NYC360.Application.Features.RssSources.Commands.ConnectRequest;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Dashboard.News;

public class ReviewNewsRssRequestEndpoint(
    IMediator mediator,
    INewsAuthorizationService newsAuthorizationService)
    : Endpoint<UpdateRssConnectionRequestStatusRequest, StandardResponse>
{
    public override void Configure()
    {
        Put("/news-dashboard/rss/requests/review");
    }

    public override async Task HandleAsync(UpdateRssConnectionRequestStatusRequest req, CancellationToken ct)
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
            new RssFeedConnectionRequestUpdateCommand(req.Id, req.Status, req.AdminNote), ct);

        await Send.OkAsync(result, ct);
    }
}
