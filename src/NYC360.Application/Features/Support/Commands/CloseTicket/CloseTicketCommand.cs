using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Support.Commands.CloseTicket;

public record CloseTicketCommand(int TicketId, int AdminUserId) : IRequest<StandardResponse>;