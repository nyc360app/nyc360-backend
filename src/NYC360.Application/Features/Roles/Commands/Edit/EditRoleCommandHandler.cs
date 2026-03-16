using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NYC360.Domain.Constants;
using NYC360.Domain.Entities;
using System.Security.Claims;
using MediatR;
using NYC360.Domain.Entities.User;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Roles.Commands.Edit;

public class EditRoleCommandHandler(
    RoleManager<ApplicationRole> roleManager,
    ILogger<EditRoleCommandHandler> logger
) : IRequestHandler<EditRoleCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(EditRoleCommand request, CancellationToken ct)
    {
        var role = await roleManager.FindByIdAsync(request.RoleId.ToString());
        if (role == null)
            return StandardResponse.Failure(new("role.notfound", "Role not found."));

        // Update role name if changed
        if (!string.Equals(role.Name, request.Name, StringComparison.OrdinalIgnoreCase))
        {
            role.Name = request.Name;
            var updateResult = await roleManager.UpdateAsync(role);
            if (!updateResult.Succeeded)
                return StandardResponse.Failure(new("role.update.failed",
                    string.Join(", ", updateResult.Errors.Select(e => e.Description))));
        }

        // Update permissions
        var allPermissions = Permissions.GetAllPermissions();
        var currentClaims = await roleManager.GetClaimsAsync(role);
        var currentPermissions = currentClaims
            .Where(c => c.Type == Permissions.PermissionClaimType)
            .Select(c => c.Value)
            .ToList();

        // Remove permissions that are no longer assigned
        foreach (var permission in currentPermissions.Except(request.Permissions))
        {
            var claim = currentClaims.First(c => c.Type == Permissions.PermissionClaimType && c.Value == permission);
            await roleManager.RemoveClaimAsync(role, claim);
        }

        // Add new permissions
        foreach (var permission in request.Permissions.Except(currentPermissions).Intersect(allPermissions))
        {
            await roleManager.AddClaimAsync(role, new Claim(Permissions.PermissionClaimType, permission));
        }

        logger.LogInformation("Updated role {RoleId} with {Count} permissions", role.Id, request.Permissions.Count);
        return StandardResponse.Success();
    }
}