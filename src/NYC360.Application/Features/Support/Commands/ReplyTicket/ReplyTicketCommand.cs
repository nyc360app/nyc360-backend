using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Support.Commands.ReplyTicket;

public record ReplyTicketCommand(int TicketId, int AdminUserId, string ReplyMessage) 
    : IRequest<StandardResponse>;