using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Users.Commands.Roles.UpdatePermissions;

public record UpdateUserPermissionsCommand(int UserId, List<string> Permissions ) 
    : IRequest<StandardResponse>;