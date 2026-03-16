using NYC360.Application.Features.Housing.Commands.UpdateRequestStatus;
using NYC360.API.Models.Housing;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.Housing;

public class UpdateHousingRequestStatusEndpoint(IMediator mediator) 
    : Endpoint<UpdateHousingRequestStatusRequest, StandardResponse>
{
    public override void Configure()
    {
        Put("/housing/agent/request/{RequestId}/status");
    }
    
    public override async Task HandleAsync(UpdateHousingRequestStatusRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var command = new UpdateHousingRequestStatusCommand(
            userId.Value,
            req.RequestId,
            req.Status
        );

        var response = await mediator.Send(command, ct);

        await Send.OkAsync(response, cancellation: ct);
    }
}
