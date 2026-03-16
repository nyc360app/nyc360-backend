using NYC360.Domain.Enums;
using MediatR;
using NYC360.Domain.Enums.Posts;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Posts.Commands.Interaction;

public record PostInteractionCommand(
    int UserId,
    int PostId,
    InteractionType Interaction
) : IRequest<StandardResponse<InteractionType?>>;