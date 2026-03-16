using MediatR;
using NYC360.Domain.Dtos;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Roles.Queries.List;

public record GetRolesQuery() : IRequest<StandardResponse<List<RoleDto>>>;