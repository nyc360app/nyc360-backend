using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.SpaceListings.Commands.Cancel;

public record CancelSpaceListingCommand(
    int ListingId,
    int UserId
) : IRequest<StandardResponse>;
