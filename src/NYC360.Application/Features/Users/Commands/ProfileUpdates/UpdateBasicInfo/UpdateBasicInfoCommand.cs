using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Users.Commands.ProfileUpdates.UpdateBasicInfo;

public record UpdateBasicInfoCommand(
    int UserId, 
    string FirstName, 
    string LastName, 
    string Headline, 
    string Bio, 
    int? LocationId
) : IRequest<StandardResponse>;