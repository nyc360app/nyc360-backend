using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Users.Commands.ProfileUpdates.UpdatePosition;

public record UpdatePositionCommand(
    int UserId,
    int PositionId,
    string Title,
    string Company,
    DateTime StartDate,
    DateTime? EndDate,
    bool IsCurrent
) : IRequest<StandardResponse>;