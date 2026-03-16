using Microsoft.AspNetCore.Identity;
using NYC360.Domain.Entities;
using MediatR;
using NYC360.Domain.Entities.User;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Users.Commands.Roles.UpdateRoles;

public class UpdateUserRolesCommandHandler(
    UserManager<ApplicationUser> userManager,
    RoleManager<ApplicationRole> roleManager ) 
    : IRequestHandler<UpdateUserRolesCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(UpdateUserRolesCommand request, CancellationToken ct)
    {
        if (request.UserId == request.ChangerId)
        {
            return StandardResponse.Failure(new("user.selfrole", "You cannot change your own role."));
        }

        var user = await userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
            return StandardResponse.Failure(new("user.notfound", "User not found."));

        var invalidRoles = await roleManager.RoleExistsAsync(request.RoleName);
        if (!invalidRoles)
            return StandardResponse.Failure(new("roles.invalid", $"Invalid role: {string.Join(", ", invalidRoles)}"));

        var currentRoles = await userManager.GetRolesAsync(user);

        if (currentRoles.Contains("SuperAdmin"))
        {
            return StandardResponse.Failure(new ApiError("user.protected", "You cannot change Super Admin role."));
        }

        if (currentRoles.Contains(request.RoleName))
        {
            return StandardResponse.Failure(new ApiError("user.already_has_role", "User already has this role."));
        }
        await userManager.RemoveFromRolesAsync(user, currentRoles);
        await userManager.AddToRoleAsync(user, request.RoleName);

        return StandardResponse.Success();
    }
}