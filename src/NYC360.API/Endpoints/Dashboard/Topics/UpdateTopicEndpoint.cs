using FastEndpoints;
using MediatR;
using NYC360.Application.Features.Topics.Commands.Update;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Dashboard.Topics;

public class UpdateTopicEndpoint(IMediator mediator) : Endpoint<UpdateTopicCommand, StandardResponse>
{
    public override void Configure()
    {
        Put("/topics-dashboard/update");
        Permissions(Domain.Constants.Permissions.Topics.Edit);
    }

    public override async Task HandleAsync(UpdateTopicCommand req, CancellationToken ct)
    {
        var result = await mediator.Send(req, ct);
        await Send.OkAsync(result, ct);
    }
}
