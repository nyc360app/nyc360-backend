using NYC360.Application.Features.Posts.Queries.GetUniversalFeed;
using NYC360.Domain.Dtos.Posts;
using NYC360.API.Models.Post;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.Posts;

public class GetUniversalFeedEndpoint(IMediator mediator) : Endpoint<GetUniversalFeedRequest, PagedResponse<PostDto>>
{
    public override void Configure()
    {
        Get("/posts/feed");
    }

    public override async Task HandleAsync(GetUniversalFeedRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var query = new GetUniversalFeedQuery(
            userId.Value,
            req.Category,
            req.LocationId,
            req.Search,
            req.Type,
            req.Page,
            req.PageSize);
        var result = await mediator.Send(query, ct);
        
        await Send.OkAsync(result, ct);
    }
}