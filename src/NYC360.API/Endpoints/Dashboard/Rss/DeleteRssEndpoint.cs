using NYC360.Application.Features.RssSources.Commands.Delete;
using NYC360.API.Models.RssSources;
using FastEndpoints;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Dashboard.Rss;

public class DeleteRssListEndpoint(IMediator mediator) : Endpoint<RssSourceDeleteRequest, StandardResponse>
{
    public override void Configure()
    {
        Delete("/rss-dashboard/delete/{SourceId:int}");
        Permissions(Domain.Constants.Permissions.RssFeeds.Delete);
    }
    public override async Task HandleAsync(RssSourceDeleteRequest request, CancellationToken ct)
    {
        var command = new RssSourceDeleteCommand(request.SourceId);
        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, ct);
    }
}