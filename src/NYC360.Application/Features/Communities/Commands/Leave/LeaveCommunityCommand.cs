using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Communities.Commands.Leave;

public record LeaveCommunityCommand(
    int UserId,
    int CommunityId
) : IRequest<StandardResponse>;