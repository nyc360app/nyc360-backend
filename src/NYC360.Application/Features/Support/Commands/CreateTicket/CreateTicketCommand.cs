using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Support.Commands.CreateTicket;

public record CreateTicketCommand(int? UserId, string? Email, string? Name, string Subject, string Message) 
    : IRequest<StandardResponse>;