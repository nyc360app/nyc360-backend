using NYC360.Domain.Enums;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Posts.Commands.SubmitFlag;

public record SubmitPostFlagCommand(
    int UserId,
    int PostId,
    FlagReasonType Reason,
    string? Details
) : IRequest<StandardResponse>;