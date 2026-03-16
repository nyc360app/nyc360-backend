using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Roles.Commands.Edit;

public record EditRoleCommand(int RoleId, string Name, List<string> Permissions, int ContentLimit) 
    : IRequest<StandardResponse>;