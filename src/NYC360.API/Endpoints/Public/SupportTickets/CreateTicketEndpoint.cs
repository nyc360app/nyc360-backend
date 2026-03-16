using NYC360.Application.Features.Support.Commands.CreateTicket;
using NYC360.API.Models.SupportTickets;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.SupportTickets;

public class CreateTicketEndpoint(IMediator mediator) : Endpoint<CreateTicketRequest, StandardResponse>
{
    public override void Configure()
    {
        Post("/support-tickets/ticket/create/private");
    }
    
    public override async Task HandleAsync(CreateTicketRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.OkAsync(StandardResponse.Failure(
                new ApiError("auth.invalidEmail", "Email not found")
            ), ct);
            return;
        }

        var command = new CreateTicketCommand(
            userId.Value,
            null,
            null,
            req.Subject,
            req.Message
        );
        
        var result = await mediator.Send(command, ct);

        await Send.OkAsync(result, ct);
    }
}