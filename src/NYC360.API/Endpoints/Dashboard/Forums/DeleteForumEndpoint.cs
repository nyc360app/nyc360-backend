using FastEndpoints;
using MediatR;
using NYC360.Application.Features.Forums.Commands.DeleteForum;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Dashboard.Forums;

public class DeleteForumEndpoint(IMediator mediator) : EndpointWithoutRequest<StandardResponse>
{
    public override void Configure()
    {
        Delete("/forums-dashboard/{Id}");
        Permissions(Domain.Constants.Permissions.Forums.Delete);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<int>("Id");
        var result = await mediator.Send(new DeleteForumCommand(id), ct);
        await Send.OkAsync(result, ct);
    }
}
