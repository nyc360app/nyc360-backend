using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.SpaceListings.Commands.Resubmit;

public record ResubmitSpaceListingCommand(
    int ListingId,
    int UserId
) : IRequest<StandardResponse>;
