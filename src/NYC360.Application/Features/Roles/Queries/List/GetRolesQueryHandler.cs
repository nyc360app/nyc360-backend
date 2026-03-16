using Microsoft.AspNetCore.Identity;
using NYC360.Domain.Constants;
using NYC360.Domain.Entities;
using MediatR;
using NYC360.Domain.Dtos;
using NYC360.Domain.Entities.User;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Roles.Queries.List;

public class GetRolesQueryHandler(RoleManager<ApplicationRole> roleManager)
    : IRequestHandler<GetRolesQuery, StandardResponse<List<RoleDto>>>
{
    public async Task<StandardResponse<List<RoleDto>>> Handle(GetRolesQuery request, CancellationToken ct)
    {
        var roles = roleManager.Roles.ToList();
        var roleDtos = new List<RoleDto>();

        foreach (var role in roles)
        {
            var claims = await roleManager.GetClaimsAsync(role);
            var permissions = claims
                .Where(c => c.Type == Permissions.PermissionClaimType)
                .Select(c => c.Value)
                .ToList();

            roleDtos.Add(RoleDto.Map(role, permissions));
        }

        return StandardResponse<List<RoleDto>>.Success(roleDtos);
    }
}