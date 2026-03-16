using NYC360.Application.Features.Professions.Commands.Withdraw;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.Professions;

public class WithdrawApplicationEndpoint(IMediator mediator) : EndpointWithoutRequest<StandardResponse>
{
    public override void Configure()
    {
        Post("/professions/applications/{ApplicationId}/withdraw");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }
        
        var applicationId = Route<int>("ApplicationId");
        var command = new WithdrawApplicationCommand(userId.Value, applicationId);
        var result = await mediator.Send(command, ct);

        await Send.OkAsync(result, ct);
    }
}