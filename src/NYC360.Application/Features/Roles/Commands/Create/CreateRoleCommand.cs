using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Roles.Commands.Create;

public record CreateRoleCommand(string Name, List<string> Permissions, int ContentLimit) 
    : IRequest<StandardResponse>;