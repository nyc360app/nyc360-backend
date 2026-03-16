using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Roles.Commands.Delete;

public record DeleteRoleCommand(int RoleId) : IRequest<StandardResponse>;