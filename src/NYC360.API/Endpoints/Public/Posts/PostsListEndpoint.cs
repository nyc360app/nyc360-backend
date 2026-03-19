using NYC360.Application.Features.Posts.Queries.List;
using NYC360.API.Models.Post;
using FastEndpoints;
using MediatR;
using NYC360.API.Extensions;
using NYC360.Domain.Dtos.Posts;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Public.Posts;

public class PostsListEndpoint(IMediator mediator) : Endpoint<PostsGetPagedRequest, PagedResponse<PostDto>>
{
    public override void Configure()
    {
        Get("/posts/list");
    }

    public override async Task HandleAsync(PostsGetPagedRequest req, CancellationToken ct)
    {
        var query = new PostsGetPagedQuery(
            User.GetId(),
            req.Page,
            req.PageSize,
            req.Category,
            req.Search,
            req.PostType,
            req.SourceType,
            false
        );

        var result = await mediator.Send(query, ct);
        await Send.OkAsync(result, cancellation: ct);
    }
}
