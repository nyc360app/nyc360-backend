using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Professions.Commands.Withdraw;

public record WithdrawApplicationCommand(
    int UserId,
    int ApplicationId
) : IRequest<StandardResponse>;