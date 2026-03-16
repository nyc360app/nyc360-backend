using NYC360.Application.Features.Posts.Queries.GetMyPostsByCategory;
using NYC360.Domain.Dtos.Posts;
using NYC360.Domain.Wrappers;
using NYC360.API.Models.Post;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.Posts;

public class GetMyPostsByCategoryEndpoint(IMediator mediator) : Endpoint<GetPostsByCategoryRequest, PagedResponse<PostDto>>
{
    public override void Configure()
    {
        Get("/posts/me/category/{Category}");
    }

    public override async Task HandleAsync(GetPostsByCategoryRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }
        
        var query = new GetMyPostsByCategoryQuery(req.Category, req.Page, req.PageSize, userId.Value);
        var result = await mediator.Send(query, ct);
        await Send.OkAsync(result, ct);
    }
}
