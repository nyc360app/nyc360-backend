using NYC360.Domain.Wrappers;
using NYC360.Domain.Enums;
using MediatR;

namespace NYC360.Application.Features.Users.Commands.ProfileUpdates.AddSocialLink;

public record AddSocialLinkCommand(
    int UserId, 
    SocialPlatform Platform, 
    string Url
) : IRequest<StandardResponse<int>>;