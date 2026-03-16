using NYC360.Domain.Enums;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Flags.Commands.ReviewFlag;

public record ReviewFlagCommand(
    int UserId,
    int FlagId,
    FlagStatus NewStatus,
    string? AdminNote
) : IRequest<StandardResponse>;