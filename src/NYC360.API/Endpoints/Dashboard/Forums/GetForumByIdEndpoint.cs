using FastEndpoints;
using MediatR;
using NYC360.Application.Features.Forums.Queries.GetForumById;
using NYC360.Domain.Dtos.Forums;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Dashboard.Forums;

public class GetForumByIdEndpoint(IMediator mediator) : EndpointWithoutRequest<StandardResponse<ForumDto>>
{
    public override void Configure()
    {
        Get("/forums-dashboard/{Id}");
        Permissions(Domain.Constants.Permissions.Forums.View);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<int>("Id");
        var result = await mediator.Send(new GetForumByIdQuery(id), ct);
        await Send.OkAsync(result, ct);
    }
}
