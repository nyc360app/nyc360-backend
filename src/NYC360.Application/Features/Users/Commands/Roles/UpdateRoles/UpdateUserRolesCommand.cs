using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Users.Commands.Roles.UpdateRoles;

public record UpdateUserRolesCommand(int UserId, string RoleName, int ChangerId, string ChangerRole) : IRequest<StandardResponse>;