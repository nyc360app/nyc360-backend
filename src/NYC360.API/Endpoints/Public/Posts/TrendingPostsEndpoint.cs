using NYC360.Application.Features.Posts.Queries.Trending;
using NYC360.Domain.Dtos.Posts;
using NYC360.API.Models.Post;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Public.Posts;

public class TrendingPostsEndpoint(IMediator mediator) : Endpoint<TrendingPostsGetRequest, PagedResponse<PostDto>>
{
    public override void Configure()
    {
        Get("/posts/trending");
    }

    public override async Task HandleAsync(TrendingPostsGetRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        var interests = User.GetInterests();
        var query = new TrendingPostsQuery(userId, interests, req.Page, req.PageSize);
        var result = await mediator.Send(query, ct);
        await Send.OkAsync(result, cancellation: ct);
    }
}