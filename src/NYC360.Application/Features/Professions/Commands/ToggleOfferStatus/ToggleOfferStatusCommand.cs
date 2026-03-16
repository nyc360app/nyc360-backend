using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Professions.Commands.ToggleOfferStatus;

public record ToggleOfferStatusCommand(
    int UserId,
    int OfferId
) : IRequest<StandardResponse>;