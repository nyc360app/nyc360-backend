using NYC360.Domain.Enums.Housing;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Housing.Commands.UpdateRequestStatus;

public record UpdateHousingRequestStatusCommand(
    int AgentUserId,
    int RequestId,
    HousingRequestStatus Status
) : IRequest<StandardResponse>;
