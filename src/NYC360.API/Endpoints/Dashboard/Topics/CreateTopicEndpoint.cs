using FastEndpoints;
using MediatR;
using NYC360.Application.Features.Topics.Commands.Create;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Dashboard.Topics;

public class CreateTopicEndpoint(IMediator mediator) : Endpoint<CreateTopicCommand, StandardResponse<int>>
{
    public override void Configure()
    {
        Post("/topics-dashboard/create");
        Permissions(Domain.Constants.Permissions.Topics.Create);
    }

    public override async Task HandleAsync(CreateTopicCommand req, CancellationToken ct)
    {
        var result = await mediator.Send(req, ct);
        await Send.OkAsync(result, ct);
    }
}
