using FastEndpoints;
using MediatR;
using NYC360.Application.Features.RssSources.Queries.Test;
using NYC360.Domain.Dtos.Rss;
using NYC360.Domain.Wrappers;
using NYC360.API.Models.RssSources;

namespace NYC360.API.Endpoints.Dashboard.Rss;

public class TestRssEndpoint(IMediator mediator) : Endpoint<RssSourceTestRequest, StandardResponse<RssSourceDto>>
{
    public override void Configure()
    {
        Get("/rss-dashboard/test");
        Permissions(Domain.Constants.Permissions.RssFeeds.Create);
    }
    
    public override async Task HandleAsync(RssSourceTestRequest request, CancellationToken ct)
    {
        var query = new TestRssSourceQuery(request.Url);
        var result = await mediator.Send(query, ct);
        await Send.OkAsync(result, ct);
    }
}
