using MediatR;
using NYC360.Domain.Dtos.Posts;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Posts.Commands.FeatureToggle;

public sealed record PostFeatureToggleCommand(
    int PostId,
    bool IsFeatured,
    int ProcessedByUserId
) : IRequest<StandardResponse<PostFeatureStatusDto>>;
