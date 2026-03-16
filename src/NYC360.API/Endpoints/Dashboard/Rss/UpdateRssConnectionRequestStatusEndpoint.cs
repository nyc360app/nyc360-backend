using NYC360.Domain.Enums;
using FastEndpoints;
using MediatR;
using NYC360.Application.Features.RssSources.Commands.ConnectRequest;
using NYC360.Domain.Wrappers;
using NYC360.API.Models.RssSources;

namespace NYC360.API.Endpoints.Dashboard.Rss;

public class UpdateRssConnectionRequestStatusEndpoint(IMediator mediator) : Endpoint<UpdateRssConnectionRequestStatusRequest, StandardResponse>
{
    public override void Configure()
    {
        Put("/rss-dashboard/requests/update");
        Permissions(Domain.Constants.Permissions.RssFeeds.Edit);
    }
    
    public override async Task HandleAsync(UpdateRssConnectionRequestStatusRequest request, CancellationToken ct)
    {
        var command = new RssFeedConnectionRequestUpdateCommand(
            request.Id,
            request.Status,
            request.AdminNote);
            
        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, ct);
    }
}
