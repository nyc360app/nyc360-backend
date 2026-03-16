using MediatR;
using NYC360.Domain.Dtos;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Roles.Queries.ById;

public record GetRoleByIdQuery(int RoleId) : IRequest<StandardResponse<RoleDto>>;