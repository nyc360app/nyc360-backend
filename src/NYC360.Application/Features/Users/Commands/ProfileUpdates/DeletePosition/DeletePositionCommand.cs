using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Users.Commands.ProfileUpdates.DeletePosition;

public record DeletePositionCommand(
    int UserId, 
    int PositionId
) : IRequest<StandardResponse>;