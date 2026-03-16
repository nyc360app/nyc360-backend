using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NYC360.Domain.Constants;
using System.Security.Claims;
using NYC360.Domain.Entities;
using MediatR;
using NYC360.Domain.Entities.User;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Users.Commands.Roles.UpdatePermissions;

public class UpdateUserPermissionsCommandHandler(
    UserManager<ApplicationUser> userManager,
    ILogger<UpdateUserPermissionsCommandHandler> logger
) : IRequestHandler<UpdateUserPermissionsCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(UpdateUserPermissionsCommand request, CancellationToken ct)
    {
        var user = await userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
            return StandardResponse.Failure(new("user.notfound", "User not found."));

        var allPermissions = Permissions.GetAllPermissions();
        var invalidPermissions = request.Permissions.Except(allPermissions).ToList();
        if (invalidPermissions.Any())
            return StandardResponse.Failure(new("permissions.invalid", $"Invalid permissions: {string.Join(", ", invalidPermissions)}"));

        var currentClaims = await userManager.GetClaimsAsync(user);
        var currentPermissions = currentClaims
            .Where(c => c.Type == Permissions.PermissionClaimType)
            .Select(c => c.Value)
            .ToList();

        // Remove permissions no longer assigned
        foreach (var permission in currentPermissions.Except(request.Permissions))
        {
            var claim = currentClaims.First(c => c.Type == Permissions.PermissionClaimType && c.Value == permission);
            await userManager.RemoveClaimAsync(user, claim);
        }

        // Add new permissions
        foreach (var permission in request.Permissions.Except(currentPermissions))
        {
            await userManager.AddClaimAsync(user, new Claim(Permissions.PermissionClaimType, permission));
        }

        logger.LogInformation("Updated user-level permissions for user {UserId}: {Count} permissions", user.Id, request.Permissions.Count);
        return StandardResponse.Success();
    }
}