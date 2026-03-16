using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Users.Commands.ProfileUpdates.AddPosition;

public record AddPositionCommand(
    int UserId,
    string Title,
    string Company,
    DateTime StartDate,
    DateTime? EndDate,
    bool IsCurrent
) : IRequest<StandardResponse<int>>;