using NYC360.Application.Features.Posts.Queries.GetMyPostsAnalysisByCategory;
using NYC360.Domain.Dtos.Posts;
using NYC360.Domain.Wrappers;
using NYC360.API.Models.Post;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.Posts;

public class GetMyPostsAnalysisByCategoryEndpoint(IMediator mediator) : Endpoint<GetPostsByCategoryRequest, StandardResponse<PostAnalysisDto>>
{
    public override void Configure()
    {
        Get("/posts/me/category/{Category}/analysis");
    }

    public override async Task HandleAsync(GetPostsByCategoryRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }
        
        var query = new GetMyPostsAnalysisByCategoryQuery(req.Category, userId.Value);
        var result = await mediator.Send(query, ct);
        await Send.OkAsync(result, ct);
    }
}
