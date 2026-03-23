using MediatR;
using NYC360.Domain.Enums.SpaceListings;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.SpaceListings.Commands.Review;

public record ReviewSpaceListingCommand(
    int ListingId,
    int ReviewerUserId,
    SpaceListingStatus Decision,
    string? ModerationNote
) : IRequest<StandardResponse>;
