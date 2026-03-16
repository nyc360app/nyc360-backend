using NYC360.Application.Features.Posts.Queries.GetSavedPosts;
using NYC360.Domain.Dtos.Posts;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using NYC360.API.Models.Post;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.Posts;

public class GetSavedPostsEndpoint(IMediator mediator) 
    : Endpoint<GetSavedPostsRequest, PagedResponse<PostDto>>
{
    public override void Configure()
    {
        Get("/posts/saved");
    }

    public override async Task HandleAsync(GetSavedPostsRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var query = new GetSavedPostsQuery(userId.Value, req.Page, req.PageSize, req.Category);
        var result = await mediator.Send(query, ct);

        await Send.OkAsync(result, ct);
    }
}