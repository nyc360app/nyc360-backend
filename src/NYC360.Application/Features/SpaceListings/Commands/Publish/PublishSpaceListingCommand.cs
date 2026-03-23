using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.SpaceListings.Commands.Publish;

public record PublishSpaceListingCommand(
    int ListingId,
    int ReviewerUserId
) : IRequest<StandardResponse>;
