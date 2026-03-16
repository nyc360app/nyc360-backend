using NYC360.API.Models.RssSources;
using FastEndpoints;
using MediatR;
using NYC360.Application.Features.RssSources.Commands.Create;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Dashboard.Rss;

public class CreateRssEndpoint(IMediator mediator) : Endpoint<RssSourceCreateRequest, StandardResponse>
{
    public override void Configure()
    {
        Post("/rss-dashboard/create");
        Permissions(Domain.Constants.Permissions.RssFeeds.Create);
        AllowFileUploads();
    }
    
    public override async Task HandleAsync(RssSourceCreateRequest request, CancellationToken ct)
    {
        var command = new RssSourceCreateCommand(
            request.Url, 
            request.Category, 
            request.Name, 
            request.Description, 
            request.ImageUrl,
            request.Image);
            
        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, ct);
    }
}