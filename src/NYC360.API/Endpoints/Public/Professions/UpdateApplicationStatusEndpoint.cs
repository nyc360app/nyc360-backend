using NYC360.Application.Features.Professions.Commands.UpdateApplicationStatus;
using NYC360.API.Models.Professions;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.Professions;

public class UpdateApplicationStatusEndpoint(IMediator mediator) 
    : Endpoint<UpdateApplicationStatusRequest, StandardResponse>
{
    public override void Configure()
    {
        Put("/professions/offers/update-application");
    }

    public override async Task HandleAsync(UpdateApplicationStatusRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }
        
        var result = await mediator.Send(new UpdateApplicationStatusCommand(
            userId!.Value,
            req.ApplicationId,
            req.Status
        ), ct);

        await Send.OkAsync(result, ct);
    }
}