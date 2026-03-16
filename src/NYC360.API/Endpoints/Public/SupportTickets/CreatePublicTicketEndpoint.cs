using FastEndpoints;
using MediatR;
using NYC360.API.Extensions;
using NYC360.API.Models.SupportTickets;
using NYC360.Application.Features.Support.Commands.CreateTicket;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Public.SupportTickets;

public class CreatePublicTicketEndpoint(IMediator mediator) : Endpoint<CreatePublicTicketRequest, StandardResponse>
{
    public override void Configure()
    {
        Post("/support-tickets/ticket/create/public");
        AllowAnonymous();
    }
    
    public override async Task HandleAsync(CreatePublicTicketRequest req, CancellationToken ct)
    {
        var command = new CreateTicketCommand(
            null,
            req.Email,
            req.Name,
            req.Subject,
            req.Message
        );
        
        var result = await mediator.Send(command, ct);

        await Send.OkAsync(result, ct);
    }
}