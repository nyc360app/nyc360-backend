using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.News.Commands.ReviewSubmission;

public record ReviewNewsSubmissionCommand(
    int ModeratorUserId,
    int PostId,
    bool Approved,
    string? ModerationNote
) : IRequest<StandardResponse>;
