using NYC360.Domain.Enums;
using FastEndpoints;
using MediatR;
using NYC360.Application.Features.RssSources.Commands.ConnectRequest;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using NYC360.API.Models.RssSources;

namespace NYC360.API.Endpoints.Public.Rss;

public class CreateRssConnectionRequestEndpoint(IMediator mediator) : Endpoint<RssFeedConnectionRequestRequest, StandardResponse>
{
    public override void Configure()
    {
        Post("/rss/connect");
    }
    
    public override async Task HandleAsync(RssFeedConnectionRequestRequest request, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var command = new RssFeedConnectionRequestCreateCommand(
            request.Url, 
            request.Category, 
            request.Name, 
            request.Description, 
            request.ImageUrl,
            userId.Value);
            
        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, ct);
    }
}
