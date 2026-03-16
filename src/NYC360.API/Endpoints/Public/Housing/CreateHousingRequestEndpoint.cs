using NYC360.Application.Features.Housing.Commands.CreateAgentRequest;
using NYC360.API.Models.Housing;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.Housing;

public class CreateHousingRequestEndpoint(IMediator mediator) 
    : Endpoint<CreateAgentRequestRequest, StandardResponse<int>>
{
    public override void Configure()
    {
        Post("/housing/agent/request");
    }
    
    public override async Task HandleAsync(CreateAgentRequestRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var command = new CreateAgentRequestCommand(
            userId.Value,
            req.PostId,
            req.Name,
            req.Email,
            req.PhoneNumber,
            req.PreferredContactType,
            req.PreferredContactDate,
            req.PreferredContactTime,
            req.HouseholdType,
            req.MoveInDate,
            req.MoveOutDate,
            req.ScheduleVirtualDate,
            req.ScheduleVirtualTimeWindow,
            req.InPersonTourDate,
            req.InPersonTourTimeWindow,
            req.Message
        );

        var response = await mediator.Send(command, ct);

        await Send.OkAsync(response, cancellation: ct);
    }
}