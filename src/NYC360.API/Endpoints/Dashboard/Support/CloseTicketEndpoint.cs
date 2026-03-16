using NYC360.Application.Features.Support.Commands.CloseTicket;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Dashboard.Support;

public class CloseTicketEndpoint(IMediator mediator) : EndpointWithoutRequest<StandardResponse>
{
    public override void Configure()
    {
        Patch("/support-dashboard/{TicketId}/close");
        Permissions(Domain.Constants.Permissions.SupportTickets.Close);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var ticketId = Route<int>("TicketId");
        var adminId = User.GetId();
        if (adminId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }
        
        var command = new CloseTicketCommand(ticketId, adminId.Value);
        var result = await mediator.Send(command, ct);

        await Send.OkAsync(result, ct);
    }
}