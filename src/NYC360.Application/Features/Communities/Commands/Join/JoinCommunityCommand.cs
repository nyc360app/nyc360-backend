using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Communities.Commands.Join;

public record JoinCommunityCommand(
    int UserId,
    int CommunityId
) : IRequest<StandardResponse>;