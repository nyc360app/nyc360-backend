using FastEndpoints;
using MediatR;
using NYC360.API.Extensions;
using NYC360.API.Models.News;
using NYC360.Application.Contracts.Services;
using NYC360.Application.Features.RssSources.Commands.Create;
using NYC360.Domain.Enums;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Dashboard.News;

public class CreateNewsRssSourceEndpoint(
    IMediator mediator,
    INewsAuthorizationService newsAuthorizationService)
    : Endpoint<CreateNewsRssSourceRequest, StandardResponse>
{
    public override void Configure()
    {
        Post("/news-dashboard/rss/create");
        AllowFileUploads();
    }

    public override async Task HandleAsync(CreateNewsRssSourceRequest req, CancellationToken ct)
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

        var result = await mediator.Send(
            new RssSourceCreateCommand(
                req.Url,
                Category.News,
                req.Name,
                req.Description,
                req.ImageUrl,
                req.Image),
            ct);

        await Send.OkAsync(result, ct);
    }
}
