using NYC360.Domain.Enums.Professions;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Professions.Commands.UpdateApplicationStatus;

public record UpdateApplicationStatusCommand(
    int UserId,
    int ApplicationId,
    ApplicationStatus NewStatus
) : IRequest<StandardResponse>;