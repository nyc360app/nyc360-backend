using FastEndpoints;
using MediatR;
using NYC360.Application.Features.Topics.Commands.Delete;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Dashboard.Topics;

public class DeleteTopicEndpoint(IMediator mediator) : EndpointWithoutRequest<StandardResponse>
{
    public override void Configure()
    {
        Delete("/topics-dashboard/delete/{id}");
        Permissions(Domain.Constants.Permissions.Topics.Delete);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<int>("id");
        var result = await mediator.Send(new DeleteTopicCommand { Id = id }, ct);
        await Send.OkAsync(result, ct);
    }
}
