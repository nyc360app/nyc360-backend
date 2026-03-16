using NYC360.Application.Features.Tags.Commands.Delete;
using NYC360.Domain.Wrappers;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Dashboard.Tags;

public class DeleteTagEndpoint(IMediator mediator) : EndpointWithoutRequest<StandardResponse>
{
    public override void Configure()
    {
        Delete("/tags-dashboard/delete/{id}");
        Permissions(Domain.Constants.Permissions.Tags.Delete);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<int>("id");
        var command = new DeleteTagCommand(id);
        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, ct);
    }
}