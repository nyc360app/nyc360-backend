using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Housing.Commands.PublishListing;

public record PublishHouseListingCommand(
    int HouseId,
    bool IsPublished
) : IRequest<StandardResponse>;
