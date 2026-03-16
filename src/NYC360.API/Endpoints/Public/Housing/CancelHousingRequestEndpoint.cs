using NYC360.Application.Features.Housing.Commands.CancelRequest;
using NYC360.API.Models.Housing;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.Housing;

public class CancelHousingRequestEndpoint(IMediator mediator) 
    : Endpoint<CancelHousingRequestRequest, StandardResponse>
{
    public override void Configure()
    {
        Put("/housing/agent/request/{RequestId}/cancel");
    }
    
    public override async Task HandleAsync(CancelHousingRequestRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var command = new CancelHousingRequestCommand(
            userId.Value,
            req.RequestId
        );

        var response = await mediator.Send(command, ct);

        await Send.OkAsync(response, cancellation: ct);
    }
}
