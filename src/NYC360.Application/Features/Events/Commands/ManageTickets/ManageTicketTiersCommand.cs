using NYC360.Domain.Dtos.Events;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Events.Commands.ManageTickets;

public record ManageTicketTiersCommand(
    int UserId,
    int EventId,
    List<TicketTierDto> Tiers
) : IRequest<StandardResponse>;