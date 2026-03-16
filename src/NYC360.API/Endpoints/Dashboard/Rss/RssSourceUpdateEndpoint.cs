using NYC360.Application.Features.RssSources.Commands.Update;
using NYC360.API.Models.RssSources;
using FastEndpoints;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Dashboard.Rss;

public class RssSourceUpdateEndpoint(IMediator mediator) : Endpoint<RssSourceUpdateRequest, StandardResponse>
{
    public override void Configure()
    {
        Put("/rss-dashboard/update");
        Permissions(Domain.Constants.Permissions.RssFeeds.Edit);
        AllowFileUploads();
    }

    public override async Task HandleAsync(RssSourceUpdateRequest req, CancellationToken ct)
    {
        var command = new RssSourceUpdateCommand(req.Id, req.RssUrl, req.Category, req.Name, req.Description, req.Image,
            req.IsActive);
        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, cancellation: ct);
    }
}