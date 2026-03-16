using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Users.Commands.ProfileUpdates.DeleteSocialLink;

public record DeleteSocialLinkCommand(
    int UserId, 
    int LinkId
) : IRequest<StandardResponse>;