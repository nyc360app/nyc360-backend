using NYC360.Application.Features.Support.Commands.ReplyTicket;
using NYC360.API.Models.SupportTickets;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Dashboard.Support;

public class ReplyTicketEndpoint(IMediator mediator) : Endpoint<ReplyTicketRequest, StandardResponse>
{
    public override void Configure()
    {
        Post("/support-dashboard/{TicketId}/reply");
        Permissions(Domain.Constants.Permissions.SupportTickets.Reply);
    }

    public override async Task HandleAsync(ReplyTicketRequest req, CancellationToken ct)
    {
        var adminId = User.GetId();
        if (adminId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var command = new ReplyTicketCommand(req.TicketId, adminId.Value, req.ReplyMessage);
        var result = await mediator.Send(command, ct);

        await Send.OkAsync(result, ct);
    }
}