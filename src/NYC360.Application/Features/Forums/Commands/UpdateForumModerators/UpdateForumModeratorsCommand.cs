using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Forums.Commands.UpdateForumModerators;

public record UpdateForumModeratorsCommand(
    int ForumId,
    List<int> ModeratorIds
) : IRequest<StandardResponse>;
