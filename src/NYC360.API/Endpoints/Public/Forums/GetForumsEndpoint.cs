using NYC360.Application.Features.Forums.Queries.GetForums;
using NYC360.Domain.Dtos.Forums;
using NYC360.Domain.Wrappers;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.Forums;

public class GetForumsEndpoint(IMediator mediator) : EndpointWithoutRequest<StandardResponse<List<ForumDto>>>
{
    public override void Configure()
    {
        Get("/forums");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await mediator.Send(new GetForumsQuery(), ct);
        await Send.OkAsync(result, ct);
    }
}
