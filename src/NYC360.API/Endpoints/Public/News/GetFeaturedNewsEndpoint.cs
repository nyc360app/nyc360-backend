using FastEndpoints;
using MediatR;
using NYC360.API.Extensions;
using NYC360.API.Models.News;
using NYC360.Application.Features.News.Queries.GetFeaturedFeed;
using NYC360.Domain.Dtos.News;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Public.News;

public class GetFeaturedNewsEndpoint(IMediator mediator)
    : Endpoint<GetFeaturedNewsRequest, StandardResponse<NewsFeaturedFeedDto>>
{
    public override void Configure()
    {
        Get("/news/featured");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetFeaturedNewsRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        var query = new GetFeaturedNewsFeedQuery(
            userId,
            req.PageSize,
            req.Page,
            req.Cursor);
        var result = await mediator.Send(query, ct);
        await Send.OkAsync(result, ct);
    }
}
