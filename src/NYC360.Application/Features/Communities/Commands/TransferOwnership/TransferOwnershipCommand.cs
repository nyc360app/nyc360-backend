using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Communities.Commands.TransferOwnership;

public record TransferOwnershipCommand(
    int CurrentOwnerId,
    int CommunityId,
    int NewOwnerId
) : IRequest<StandardResponse<string>>;
