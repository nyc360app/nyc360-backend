using FastEndpoints;
using MediatR;
using NYC360.Application.Features.Topics.Queries.GetTopics;
using NYC360.API.Models.Topics;
using NYC360.Domain.Dtos.Topics;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Public.Topics;

public class GetTopicsEndpoint(IMediator mediator) : Endpoint<GetTopicsRequest, StandardResponse<IReadOnlyList<TopicDto>>>
{
    public override void Configure()
    {
        Get("/topics");
    }

    public override async Task HandleAsync(GetTopicsRequest req, CancellationToken ct)
    {
        var result = await mediator.Send(new GetTopicsQuery { Category = req.Category }, ct);
        await Send.OkAsync(result, ct);
    }
}
