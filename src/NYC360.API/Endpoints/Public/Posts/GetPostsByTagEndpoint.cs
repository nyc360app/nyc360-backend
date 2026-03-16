using NYC360.Application.Features.Posts.Queries.GetPostsByTag;
using NYC360.Domain.Dtos.Posts;
using NYC360.Domain.Wrappers;
using NYC360.API.Models.Post;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.Posts;

public class GetPostsByTagEndpoint(IMediator mediator) : Endpoint<GetPostsByTagRequest, PagedResponse<PostDto>>
{
    public override void Configure()
    {
        Get("/posts/tags/{Tag}");
    }

    public override async Task HandleAsync(GetPostsByTagRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.OkAsync(PagedResponse<PostDto>.Failure(
                new ApiError("auth.invalidEmail", "Email not found")
            ), ct);
            return;
        }
        
        var query = new GetPostsByTagQuery(req.Tag, req.Page, req.PageSize, userId);
        var result = await mediator.Send(query, ct);
        await Send.OkAsync(result, ct);
    }
}