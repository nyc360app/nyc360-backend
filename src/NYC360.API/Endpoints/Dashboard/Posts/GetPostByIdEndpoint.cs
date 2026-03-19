using FastEndpoints;
using MediatR;
using NYC360.API.Extensions;
using NYC360.API.Models.Post;
using NYC360.Application.Features.Posts.Queries.PostDetails;
using NYC360.Domain.Dtos.Posts;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Dashboard.Posts;

public class GetPostByIdEndpoint(IMediator mediator) : Endpoint<GetPostDetailsRequest, StandardResponse<PostDetailsDto>>
{
    public override void Configure()
    {
        Get("/posts-dashboard/{PostId}");
    }
    
    public override async Task HandleAsync(GetPostDetailsRequest req, CancellationToken ct)
    {
        var query = new PostGetDetailsQuery(req.PostId, User.GetId(), true);
        var result = await mediator.Send(query, ct);
        await Send.OkAsync(result, ct);
    }
}
