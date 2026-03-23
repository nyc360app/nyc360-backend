using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.SpaceListings.Commands.Assign;

public record AssignSpaceListingCommand(
    int ListingId,
    int ReviewerUserId,
    int AdminUserId
) : IRequest<StandardResponse>;
