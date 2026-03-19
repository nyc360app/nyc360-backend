using FastEndpoints;
using MediatR;
using NYC360.API.Extensions;
using NYC360.API.Models.News;
using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Contracts.Services;
using NYC360.Application.Features.RssSources.Commands.Delete;
using NYC360.Domain.Enums;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Dashboard.News;

public class DeleteNewsRssSourceEndpoint(
    IMediator mediator,
    INewsAuthorizationService newsAuthorizationService,
    IRssSourceRepository rssSourceRepository)
    : Endpoint<DeleteNewsRssSourceRequest, StandardResponse>
{
    public override void Configure()
    {
        Delete("/news-dashboard/rss/delete/{SourceId:int}");
    }

    public override async Task HandleAsync(DeleteNewsRssSourceRequest req, CancellationToken ct)
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

        var source = await rssSourceRepository.GetByIdAsync(req.SourceId, ct);
        if (source == null || source.Category != Category.News)
        {
            await Send.OkAsync(StandardResponse.Failure(
                new ApiError("news.rss_source_not_found", "News RSS source not found.")), ct);
            return;
        }

        var result = await mediator.Send(new RssSourceDeleteCommand(req.SourceId), ct);
        await Send.OkAsync(result, ct);
    }
}
