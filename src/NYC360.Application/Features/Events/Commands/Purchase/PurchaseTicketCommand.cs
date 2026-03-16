using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Events.Commands.Purchase;

public record PurchaseTicketCommand(int EventId, int TierId, int UserId) : IRequest<StandardResponse<string>>;
