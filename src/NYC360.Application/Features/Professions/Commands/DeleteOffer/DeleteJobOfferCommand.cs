using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Professions.Commands.DeleteOffer;

public record DeleteJobOfferCommand(
    int UserId,
    int OfferId
) : IRequest<StandardResponse>;
