using Microsoft.AspNetCore.Identity;
using NYC360.Domain.Constants;
using NYC360.Domain.Entities;
using MediatR;
using NYC360.Domain.Dtos;
using NYC360.Domain.Entities.User;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Roles.Queries.ById;

public class GetRoleByIdQueryHandler(RoleManager<ApplicationRole> roleManager) 
    : IRequestHandler<GetRoleByIdQuery, StandardResponse<RoleDto>>
{
    public async Task<StandardResponse<RoleDto>> Handle(GetRoleByIdQuery request, CancellationToken ct)
    {
        var role = await roleManager.FindByIdAsync(request.RoleId.ToString());
        if (role == null)
            return StandardResponse<RoleDto>.Failure(new("role.notfound", "Role not found."));

        var claims = await roleManager.GetClaimsAsync(role);
        var permissions = claims
            .Where(c => c.Type == Permissions.PermissionClaimType)
            .Select(c => c.Value)
            .ToList();

        var dto = RoleDto.Map(role, permissions);
        return StandardResponse<RoleDto>.Success(dto);
    }
}