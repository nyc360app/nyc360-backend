using FastEndpoints;
using MediatR;
using NYC360.Application.Features.Topics.Queries.GetPostsByTopic;
using NYC360.API.Models.Topics;
using NYC360.Domain.Dtos.Posts;
using NYC360.Domain.Dtos.Topics;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;

namespace NYC360.API.Endpoints.Public.Topics;

public class GetPostsByTopicEndpoint(IMediator mediator) : Endpoint<GetPostsByTopicRequest, PagedResponse<PostDto>>
{
    public override void Configure()
    {
        Get("/topics/{topicId}");
    }

    public override async Task HandleAsync(GetPostsByTopicRequest req, CancellationToken ct)
    {
        var result = await mediator.Send(new GetPostsByTopicQuery
        {
            UserId = User.GetId(),
            TopicId = req.TopicId,
            Page = req.Page,
            PageSize = req.PageSize
        }, ct);

        await Send.OkAsync(result, ct);
    }
}
